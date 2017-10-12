using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using SnmpSharpNet;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace SnmpClient
{
    public partial class Form1 : Form
    {
        SnmpSharpNet.OctetString community;                            
        AgentParameters param;                        
        IpAddress agent;                                
        UdpTarget target;

        public Form1()
        {
            InizializzaSNMP();
            InitializeComponent();
        }

        private void InizializzaSNMP()
        {
            community = new SnmpSharpNet.OctetString("public");                   //SNMP community name, di default "public"
            param = new AgentParameters(community);                               //Definisce parametri dell'agente, secondo community name
            param.Version = SnmpVersion.Ver1;                                     //Definizione versione SNMP 1
            agent = new IpAddress("192.168.1.96");                                //Definizione indirizzo IP della macchina
            target = new UdpTarget((IPAddress)agent, 161, 2000, 1);               //Definizione metodo con cui comunicare con la stampante: UDP, porta, timeout, retry
        }

        private void MACBotton_Click(object sender, EventArgs e)
        {
            Pdu pdu = new Pdu(PduType.Get);                                                 //Unità dati dello scambio messaggi SNMP
            pdu.VbList.Add(".1.3.6.1.2.1.3.1.1.2.1.1.192.168.1.32");                        //Indirizzo MAC
           
            SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP

            if (result != null)
            {
                if (result.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex); //errore e codice dell'errore
                }
                else
                {
                    IpLabel.Text = result.Pdu.VbList[0].Value.ToString();
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }
           
        }

        private void StampeCompleteBotton_Click(object sender, EventArgs e)
        {
            Pdu pdu = new Pdu(PduType.Get);                                                 //Unità dati dello scambio messaggi SNMP
            pdu.VbList.Add("1.3.6.1.2.1.1.1.0");                                            //Definizione generale dispositivo
            pdu.VbList.Add("1.3.6.1.2.1.1.3.0");                                            //Tempo attività
            pdu.VbList.Add("1.3.6.1.2.1.1.5.0");                                            //Nome dipositivo
            pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.19.1.0");                               //Stampe totali complete
            pdu.VbList.Add(".1.3.6.1.4.1.367.3.2.1.2.19.2.0");                              //Stampe totali complete Stampante
            pdu.VbList.Add(".1.3.6.1.4.1.367.3.2.1.2.19.4.0");                              //Stampe totali complete Fotocopiatrice

            SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP

            if (result != null)                                                             
            {
                if (result.Pdu.ErrorStatus != 0)
                { 
                        Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}",result.Pdu.ErrorStatus,result.Pdu.ErrorIndex);
                }
                else
                {
                    Console.WriteLine("Dispositivo: ({0}): {1}",SnmpConstants.GetTypeName(result.Pdu.VbList[0].Value.Type),result.Pdu.VbList[0].Value.ToString());
                    Console.WriteLine("Tempo Attività: ({0}): {1}",SnmpConstants.GetTypeName(result.Pdu.VbList[1].Value.Type),result.Pdu.VbList[1].Value.ToString());
                    Console.WriteLine("Nome Dispositivo: ({0}): {1}",SnmpConstants.GetTypeName(result.Pdu.VbList[2].Value.Type),result.Pdu.VbList[2].Value.ToString());
                    Console.WriteLine("Stampe totali complete: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[3].Value.Type), result.Pdu.VbList[3].Value.ToString());
                    Console.WriteLine("Stampe totali complete Stampante: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[4].Value.Type), result.Pdu.VbList[4].Value.ToString());
                    Console.WriteLine("Stampe totali complete Fotocopiatrice: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[5].Value.Type), result.Pdu.VbList[5].Value.ToString());
                    StampeLabel.Text = result.Pdu.VbList[3].Value.ToString();
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }      
        }
        

        private async void discoverBotton_ClickAsync(object sender, EventArgs e) //controlla la rete locale per individuare le stampanti. Sono individuate tramite risposta al broadcast
        {
            Discoverer discoverer = new Discoverer();
            discoverer.AgentFound += DiscovererAgentFound;
            Console.WriteLine("v1 discovery");
            await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, 161), new Lextm.SharpSnmpLib.OctetString("public"), 6000);    
        }

        static void DiscovererAgentFound(object sender, AgentFoundEventArgs e)
        {
            Console.WriteLine("Dispositivo Trovato IP:{0}--{1}", e.Agent, e.Variable); //Dispositivo trovato nella rete
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SNMPBotton_Click(object sender, EventArgs e)//restituisce la versione di SNMP
        {
            VersioneLabel.Text= param.Version.ToString();
        }

        private void magentaBotton_Click(object sender, EventArgs e)//controlla lo stato di inchiostro
        {
            Pdu pdu = new Pdu(PduType.Get);                                                 //Unità dati dello scambio messaggi SNMP
            pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.4");                           //Yellow
            pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.3");                           //Magenta
            pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.2");                           //Ciano
            pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.1");                           //Black

            SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP

            if (result != null)
            {
                if (result.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                    
                }
                else//riempe le progressbar
                {
                    if (Convert.ToInt32(result.Pdu.VbList[0].Value.ToString()) == -100)
                    progressBarYellow.Value = 0;
                    else
                        progressBarYellow.Value = Convert.ToInt32(result.Pdu.VbList[0].Value.ToString());

                    if (Convert.ToInt32(result.Pdu.VbList[1].Value.ToString()) == -100)
                    { progressBarMagenta.Value = 0; }
                    else
                        progressBarMagenta.Value = Convert.ToInt32(result.Pdu.VbList[1].Value.ToString());

                    if (Convert.ToInt32(result.Pdu.VbList[2].Value.ToString()) == -100)
                    { progressBarCiano.Value = 0; }
                    else
                        progressBarCiano.Value = Convert.ToInt32(result.Pdu.VbList[2].Value.ToString());

                    if (Convert.ToInt32(result.Pdu.VbList[3].Value.ToString()) == -100)
                    { progressBarBlack.Value = 0; }
                    else
                        progressBarBlack.Value = Convert.ToInt32(result.Pdu.VbList[3].Value.ToString());


                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }
        }
    }
}
