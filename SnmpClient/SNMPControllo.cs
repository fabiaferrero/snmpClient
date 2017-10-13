using System;
using System.Windows.Forms;
using System.Net;
using SnmpSharpNet;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Data.Objects;
using System.Collections;
using System.Data.Objects.DataClasses;
using System.Collections.Generic;
using System.IO;

namespace SnmpClient
{
    public partial class SNMPControllo : Form
    {
        SnmpSharpNet.OctetString community;
        AgentParameters param;
        IpAddress agent;
        UdpTarget target;
        private SNMPEntities SNMPContext;
        int id = 0;
        logger log;

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
            //pdu.VbList.Add("1.3.6.1.2.1.1.1.0");                                            //Definizione generale dispositivo
            //pdu.VbList.Add("1.3.6.1.2.1.13.0");                                             //Tempo attività
            pdu.VbList.Add("1.3.6.1.2.1.1.5.0");                                              //Nome dipositivo
            pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.19.1.0");                                 //Stampe totali complete
            //pdu.VbList.Add(".1.3.6.1.4.1.367.3.2.1.2.19.2.0");                              //Stampe totali complete Stampante
            //pdu.VbList.Add(".1.3.6.1.4.1.367.3.2.1.2.19.4.0");                              //Stampe totali complete Fotocopiatrice
            //pdu.VbList.Add(".1.3.6.1.2.1.3.1.1.2.1.1.192.168.1.32");                        //Indirizzo MAC
            //pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.4");                           //Yellow
            //pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.3");                           //Magenta
            //pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.2");                           //Ciano
            //pdu.VbList.Add("1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.1");                           //Black

            SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP

            if (result != null)
            {
                if (result.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", result.Pdu.ErrorStatus, result.Pdu.ErrorIndex);
                }
                else
                {
                    //Console.WriteLine("Dispositivo: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[0].Value.Type), result.Pdu.VbList[0].Value.ToString());
                    //Console.WriteLine("Tempo Attività: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[1].Value.Type), result.Pdu.VbList[1].Value.ToString());
                    Console.WriteLine("Nome Dispositivo: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[0].Value.Type), result.Pdu.VbList[0].Value.ToString());
                    Console.WriteLine("Stampe totali complete: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[1].Value.Type), result.Pdu.VbList[1].Value.ToString());
                    //Console.WriteLine("Stampe totali complete Stampante: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[4].Value.Type), result.Pdu.VbList[4].Value.ToString());
                    //Console.WriteLine("Stampe totali complete Fotocopiatrice: ({0}): {1}", SnmpConstants.GetTypeName(result.Pdu.VbList[5].Value.Type), result.Pdu.VbList[5].Value.ToString());

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

    

        private async void discoverBotton_ClickAsync(object sender, EventArgs e) //controlla la rete locale per individuare le stampanti. Sono individuate tramite risposta al broadcast
        { 
            Discoverer discoverer = new Discoverer();
            discoverer.AgentFound += DiscovererAgentFound;
            Console.WriteLine("v1 discovery");
            await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, 161), new Lextm.SharpSnmpLib.OctetString("public"), 6000);
        }

        static void DiscovererAgentFound(object sender, AgentFoundEventArgs e)
        {
           //((e.Agent.ToString(), e.Variable.Data.ToString()); //Dispositivo trovato nella rete
            Console.WriteLine("Dispositivo Trovato IP:{0}--{1}", e.Agent, e.Variable.Data); //Dispositivo trovato nella rete
        }

    }
}


//Aggiungere nella tabella un campo per la ricerca mirata di un dato