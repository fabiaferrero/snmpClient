using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using SnmpSharpNet;
using System.Net.Sockets;

namespace SnmpClient
{
    public partial class Form1 : Form
    {
        SecureAgentParameters param;
        IpAddress ipa;
        String ipDiscovery;
        UdpTarget target;
        Pdu pdu;
        long iplong;


        public Form1()
        {
            InizializzaSNMP();
            InitializeComponent();
        }

        public static long IP2Long(string ip)
        { 
        string[] ipBytes;
        double num = 0;
        if (!string.IsNullOrEmpty(ip))
            {
                ipBytes = ip.Split('.');
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
                }
            }
             return (long) num;
        }

        private void InizializzaSNMP()
        {
            this.param = new SecureAgentParameters();
            this.pdu = new Pdu();
            this.ipa = new IpAddress("192.168.1.96");//meglio passare tramite textbox
            this.target = new UdpTarget((IPAddress)ipa);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            SnmpV3Packet discovery = SnmpV3Packet.DiscoveryRequest(); //crea pck v3
          
            byte[] outBuffer = discovery.encode(); //buffer per invio
            byte[] inBuffer = new byte[4096];
            int  inLength = outBuffer.Length;
            EndPoint end = new IPEndPoint(1610721472, 161);
            
            s.SendTo(outBuffer, new IPEndPoint(1610721472, port: 161)); //invio pck
            s.Receive(inBuffer); //risposta da agente
            inLength = inBuffer.Length;
            discovery.decode(inBuffer, inLength);

            if (discovery.Version != SnmpVersion.Ver3)
            {
                Console.WriteLine("versione snmp errata");
                return; //versione non valida di snmp
            }
            if (discovery.Version == SnmpVersion.Ver3)
            {
                Console.WriteLine("versione snmp"+discovery.Version);
                return; //versione non valida di snmp
            }
            if (discovery.Pdu.Type != PduType.Report)
            {
                Console.WriteLine("report");
                return; //report errore
            }
            if (!discovery.Pdu.VbList[0].Oid.Equals(SnmpConstants.usmStatsUnknownEngineIDs))
            {
                Console.WriteLine("errore non riconosciuto");
                return; //errore sconosciuto
            }

            OctetString engineID = (OctetString)discovery.USM.EngineId.Clone();// Authoritative Engine ID
            Int32 engineBoots = discovery.USM.EngineBoots;// Authoritative Engine Boots
            Int32 engineTime = discovery.USM.EngineTime;// Authoritative Engine Time
            DateTime discoveryTime = DateTime.Now;// Timestamp when discovery process was completed
            Int32 maxMessageSize = discovery.MaxMessageSize;// Maximum message size agent can process

            SnmpV3Packet request = new SnmpV3Packet();// Request packet class
            request.NoAuthNoPriv(System.Text.ASCIIEncoding.UTF8.GetBytes("public")); // Set security model to NoAuthNoPriv with SecurityName "public"
            request.SetEngineId(engineID);// Set authoritative engine ID retrieved during discovery
            request.SetEngineTime(engineBoots, engineTime);// Set timeliness values retrieved during discovery
            request.MaxMessageSize = maxMessageSize;// Set maximum message size
            request.ScopedPdu.Type = PduType.Get;// Set Pdu type to Get
            request.ScopedPdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.19.1.0");// Add Oid to query to the VbList

            outBuffer = request.encode();// Encode request
            s.SendTo(outBuffer, new IPEndPoint(1610721472, 161)); //invio pck
            s.Receive(inBuffer); //risposta da agente
            request.decode(inBuffer, inLength);

            SnmpV3Packet response = new SnmpV3Packet();
            // Decode response
            response.decode(inBuffer, inLength);

            // Process response
            Console.WriteLine("Reply received to message id {0} request id {1}",
                         response.MessageId, response.Pdu.RequestId);
            Console.WriteLine("\t{0}: {1} {2}", response.ScopedPdu.VbList[0].Oid.ToString(),
              SnmpConstants.GetTypeName(response.ScopedPdu.VbList[0].Value.Type),
              response.ScopedPdu.VbList[0].Value.ToString());

        
        }

        private void Sleep(int v)
        {
            throw new NotImplementedException();
        }

        private void MACBotton_Click(object sender, EventArgs e)
        {
            this.param = new SecureAgentParameters();
            this.pdu = new Pdu();
            pdu.Type = PduType.Get;
            //Oid oidVal1 = new Oid(new int[] { 1,3,6,1,1,1,1,22 });
            //Oid oidVal1 = new Oid(new int[] { 1,3,6,1,4,1,367,3,2,1,2,19,1,0 });
            Oid oidVal1 = new Oid(new int[] { 1,3,6,1,4,1,11,2,3,9,1,1,7,0 });
            pdu.VbList.Add(oidVal1);
            param.noAuthNoPriv("public");
            SnmpV3Packet result;
            try
            {
                result = (SnmpV3Packet)target.Request(pdu, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("VUOTO: {0}", ex.Message);
                result = null;
            }
            if (result != null)
            {
                // If SNMPv3 request failed because of an error with SNMPv3 processing, PduType.Report is returned.
                //  OIDs in the response contain the error OID and counter of how many times agent has received 
                //  requests with this type of error
                if (result.ScopedPdu.Type == PduType.Report)
                {
                    Console.WriteLine("SNMPv3 report:");
                    foreach (Vb v in result.ScopedPdu.VbList)
                    {
                        Console.WriteLine("{0} -> ({1}) {2}",
                          v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                    }
                }
                else
                {
                    // Now that we know response is not a Report (or SNMPv3 error), check if there was a more generic SNMP error
                    if (result.ScopedPdu.ErrorStatus == 0)
                    {
                        // If no error is found, print all Variable Binding entries
                        foreach (Vb v in result.ScopedPdu.VbList)
                        {
                            Console.WriteLine("Dati {0} -> ({1}) {2}",
                              v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                        }
                    }
                    else
                    {
                        // SNMP error was found. Print the message and index of the variable that caused it
                        Console.WriteLine("SNMPError: {0} ({1}): {2}",
                                            SnmpError.ErrorMessage(result.ScopedPdu.ErrorStatus),
                                            result.ScopedPdu.ErrorStatus, result.ScopedPdu.ErrorIndex);
                    }
                }
            }

            //da finire trova ip da mettere nel label conrollo exception

        }

        private void StampeCompleteBotton_Click(object sender, EventArgs e)
        {
            InizializzaSNMP();
            Oid oidVal1 = new Oid(new int[] { 1, 3, 6, 1, 4, 1, 367, 3, 2, 1, 2, 24, 1, 1, 5, 3 });
            pdu.VbList.Add(oidVal1);
            param.noAuthNoPriv("public");
            SnmpV3Packet result;
            try
            {
                result = (SnmpV3Packet)target.Request(pdu, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("VUOTO: {0}", ex.Message);
                result = null;
            }
            if (result != null)
            {
                if (result.ScopedPdu.Type == PduType.Report)
                {
                  //  Console.WriteLine("RICOH report:");
                    foreach (Vb v in result.ScopedPdu.VbList)
                    {
                        //Console.WriteLine("OID COMPLETATE{0} -> ({1}) {2}",v.Oid.ToString(),SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                        StampeLabel.Text = v.Value.ToString();
                    }
                }
                else
                {
                    if (result.ScopedPdu.ErrorStatus == 0)
                    {
                        foreach (Vb v in result.ScopedPdu.VbList)
                        {
                            Console.WriteLine("{0} -> ({1}) {2}",
                              v.Oid.ToString(),
                              SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("SNMPError: {0} ({1}): {2}",
                          SnmpError.ErrorMessage(result.ScopedPdu.ErrorStatus),
                          result.ScopedPdu.ErrorStatus, result.ScopedPdu.ErrorIndex);
                    }
                }
            }
            target.Close();
        }

        private void discoverBotton_Click(object sender, EventArgs e)
        {
            List<IpAddress> ListaStampantiRete = new List<IpAddress>();

            for (int i = 0; i <= 96; i++)
            {
                String num = Convert.ToString(i);
                ipDiscovery = "192.168.1." + num;
                IpAddress ip = new IpAddress(ipDiscovery);
                UdpTarget Possibiletarget = new UdpTarget((IPAddress)ip);
                Possibiletarget.Retry = 0;
                SecureAgentParameters paramTarget = new SecureAgentParameters();
                paramTarget.noAuthNoPriv("public");
               
          
                
                if (!Possibiletarget.Discovery(paramTarget))
                {
                    Console.WriteLine("Discovery fallito");
                    Possibiletarget.Close();
                    return;
                }
                else
                {
                    Console.WriteLine("IP" + ip);
                    ListaStampantiRete.Add(ip);
                    discoverLabel.Text = Convert.ToString(ip);
                }
                
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SNMPBotton_Click(object sender, EventArgs e)
        {
            //1.3.6.1.2.1.17.2.16
            InizializzaSNMP();
            Oid oidVal1 = new Oid(new int[] { 1,3,6,1,2,1,17,2,16 });
            pdu.VbList.Add(oidVal1);
            param.noAuthNoPriv("public");
            SnmpV3Packet result;
            try
            {
                result = (SnmpV3Packet)target.Request(pdu, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("VUOTO: {0}", ex.Message);
                result = null;
            }
            if (result != null)
            {
                if (result.ScopedPdu.Type == PduType.Report)
                {

                    foreach (Vb v in result.ScopedPdu.VbList)
                    {
                        //Console.WriteLine("OID versione{0} -> ({1}) {2}", v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                        VersioneLabel.Text = v.Value.ToString();
                    }
                }
                else
                {
                    if (result.ScopedPdu.ErrorStatus == 0)
                    {
                        foreach (Vb v in result.ScopedPdu.VbList)
                        {
                            Console.WriteLine("{0} -> ({1}) {2}",
                              v.Oid.ToString(),
                              SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("SNMPError: {0} ({1}): {2}",
                          SnmpError.ErrorMessage(result.ScopedPdu.ErrorStatus),
                          result.ScopedPdu.ErrorStatus, result.ScopedPdu.ErrorIndex);
                    }
                }
            }
            target.Close();
        }

        private void magentaBotton_Click(object sender, EventArgs e)
        {
            InizializzaSNMP();
            Oid magenta = new Oid(new int[] { 1, 3, 6, 1, 4, 1, 367, 3, 2, 1, 2, 24, 11, 5, 3 });
            //Oid cyan = new Oid(new int[] { 1, 3, 6, 1, 4, 1, 367, 3, 2, 1, 2, 24, 11, 5, 2 });
            //Oid black = new Oid(new int[] { 1, 3, 6, 1, 4, 1, 367, 3, 2, 1, 2, 24, 11, 5, 1 });
            //Oid yellow = new Oid(new int[] { 1, 3, 6, 1, 4, 1, 367, 3, 2, 1, 2, 24, 11, 5, 4 });
            pdu.VbList.Add(magenta);
            //pdu.VbList.Add(cyan);
            //pdu.VbList.Add(black);
            //pdu.VbList.Add(yellow);
            param.noAuthNoPriv("public");
            SnmpV3Packet result;

            try
            {
                result = (SnmpV3Packet)target.Request(pdu, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("VUOTO: {0}", ex.Message);
                result = null;
            }
            if (result != null)
            {
                if (result.ScopedPdu.Type == PduType.Report)
                {

                    foreach (Vb v in result.ScopedPdu.VbList)
                    {
                        //Console.WriteLine("OID magenta{0} -> ({1}) {2}", v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                        //progressBarMagenta.Value = Int32.Parse(v.Value.ToString());
                    }
                }
                else
                {
                    if (result.ScopedPdu.ErrorStatus == 0)
                    {
                        foreach (Vb v in result.ScopedPdu.VbList)
                        {
                            Console.WriteLine("OID magenta{0} -> ({1}) {2}", v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                            progressBarMagenta.Value = Int32.Parse(v.Value.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("SNMPError: {0} ({1}): {2}",
                          SnmpError.ErrorMessage(result.ScopedPdu.ErrorStatus),
                          result.ScopedPdu.ErrorStatus, result.ScopedPdu.ErrorIndex);
                    }
                }
            }
            target.Close();
        }
    }
}

