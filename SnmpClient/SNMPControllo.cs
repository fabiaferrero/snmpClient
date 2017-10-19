using System;
using System.Windows.Forms;
using System.Net;
using SnmpSharpNet;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SnmpClient
{
    public partial class SNMPControllo : Form
    {
        SnmpSharpNet.OctetString community;
        AgentParameters param;
        IpAddress agent;
        UdpTarget target;
        private SNMPEntities SNMPContext=new SNMPEntities();
        int id = 0;


        public SNMPControllo()
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
        

        private void CheckStampanteBotton_Click(object sender, EventArgs e)
        {
            Pdu pdu = new Pdu(PduType.Get);                                                   //Unità dati dello scambio messaggi SNMP
            //pdu.VbList.Add("1.3.6.1.2.1.1.5.0");                                              //Nome dipositivo
            pdu.VbList.Add(".1.3.6.1.2.1.1.1.0");
            //pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.19.1.0");                                 //Stampe totali 



            var item1 = SNMPContext.OIDs.Where(x => x.Vendor == "RICOH");
            var item2 = item1.Where(x => x.Dato == "StampeComplete");
            foreach (var i in item2)
            {
                Console.WriteLine("---------------Elemento: " + i.NumeroOID);
            }


                SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP

            if (result != null)
            {
                if (result.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                }
                else
                {
                    Console.WriteLine("Nome Dispositivo: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[0].Value.Type), result.Pdu.VbList[0].Value.ToString());
                    Console.WriteLine("Stampe totali complete: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[1].Value.Type), result.Pdu.VbList[1].Value.ToString());
                    
                    SNMPContext = new SNMPEntities();

                    Stampanti risultatoSNMP = new Stampanti
                    {
                        IDStampante = id++,
                        Nome = result.Pdu.VbList[0].Value.ToString(),
                        IP = agent.ToString(),
                        NumeroStampe = Int32.Parse(result.Pdu.VbList[1].Value.ToString()),
                        Data = DateTime.Now
                    };

                    try
                    {
                        SNMPContext.Stampantis.Add(risultatoSNMP);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore nell'inserimento dei dati " + ex);
                    }
                    SNMPContext.SaveChanges();
                    MessageBox.Show("Elementi salvati nel database.");
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }

        }

        public static void Log(String ip, String disp)//costruzione messaggio del log
        {
            StreamWriter w= File.AppendText("Log.txt");
            w.Write("Dispositivo Trovato: ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("->Nome: {0}, IP:{1}", disp, ip);
            w.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            w.Flush();
            w.Close();
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
            Log(e.Agent.ToString(), e.Variable.Data.ToString()); //Dispositivo trovato nella rete e scritto nel file di log
            LeggiDatiStampante(e.Agent,e.Variable);
        }

        private static void LeggiDatiStampante(IPEndPoint agent, Variable variable)
        {
            Console.WriteLine("DATI{0},{1}", variable.Data, variable.Id);
            //ogni stampante della discovery da leggere
            //variable.data == Nome
            //Variable.id == 1.3.6.1.2.1.1.1.0
        }
    }
}


