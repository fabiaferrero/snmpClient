using System;
using System.Windows.Forms;
using System.Net;
using SnmpSharpNet;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.IO;
using System.Linq;

namespace SnmpClient
{
    public partial class SNMPControllo : Form
    {
        SnmpSharpNet.OctetString community;
        AgentParameters param;
        IpAddress agent;
        UdpTarget target;
        Entities SNMPContext = new Entities();

        public SNMPControllo()
        {
            InizializzaSNMP();
            InitializeComponent();
        }
        private void InizializzaSNMP()
        {
            community = new SnmpSharpNet.OctetString("public");              //SNMP community name, di default "public"
            param = new AgentParameters(community);                          //Definisce parametri dell'agente, secondo community name
            param.Version = SnmpVersion.Ver1;                                //Definizione versione SNMP 1
            agent = new IpAddress("192.168.1.96");                           //Definizione indirizzo IP della macchina
            target = new UdpTarget((IPAddress)agent, 161, 2000, 1);          //Definizione metodo con cui comunicare con la stampante: UDP, porta, timeout, retry
        }
       
        private int CreaId(IQueryable<int> q)
        {
            int max = 0;
            foreach (var i in q)
            {
                if (max < i)
                        max = i;
            }
            return max + 1;
        }                                             //Crea ID del prossimo elemento da inserire nel DB risprendendo dall'ultimo                                                                                                                                     //Crea l'ID del prossimo elemento da salvare nel DB, riprendendo dall'ultimo inserito
        private void SalvaDatiDiscovery(string ip, string nome)
        {
           Stampanti Dispositivo = new Stampanti //salvataggio dati richiesti su database
            {
                IDStampante = CreaId(SNMPContext.Stampantis.Select(x => x.IDStampante)),
                Nome = nome,
                IP = ip,
                Data = DateTime.Now
            };
            try
            {
                SNMPContext.Stampantis.Add(Dispositivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nell'inserimento dei dati " + ex);
            }
            try
            {
                SNMPContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nel salvataggio dei dati " + ex);
            }
        }
        private string TrovaTipoDato(IQueryable<string> t)
        {
            Console.WriteLine("tipo :"+t.ToString());
            string tipologia= System.String.Empty;
            //foreach (var z in t)
            //    Console.WriteLine("tipo :"+ z);
            return tipologia;
        }

        private void UpdateDB(SnmpV1Packet pck)
        {
            foreach(var i in pck.Pdu.VbList)
            {
                //return (from s in db.sims.Where(x => x.has_been_modified == true) select x).ToList();
                ValoriStampanti risultatoSNMP = new ValoriStampanti //salvataggio dati richiesti su database
                {
                    IDValore = CreaId(SNMPContext.ValoriStampantis.Select(x => x.IDValore)),
                    IDStampante=0,
                    TipoDato = TrovaTipoDato(SNMPContext.OIDs.Where(x => x.NumeroOID.Equals(i.Oid)).Select(x => x.Dato)),
                    Valore = i.Value.ToString(),
                    Data = DateTime.Now
                };
            }
           
            try
            {
                //SNMPContext.ValoriStampantis.Add(risultatoSNMP);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nell'inserimento dei dati " + ex);
            }
            try
            {
                SNMPContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nel salvataggio dei dati " + ex);
            }
            MessageBox.Show("Elementi salvati nel database.");
        }

        private string GetNomeStampante(SnmpV1Packet pck)                                    //Analizza il pdu per ottenere il nome della stampante per verificare supporto
        {
            string NomeStampantePdu = System.String.Empty;
            if (pck != null)
            {
                if (pck.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", pck.Pdu.ErrorStatus, pck.Pdu.ErrorIndex);
                }
                else
                {
                    Console.WriteLine("NomeStampantePdu: ({0}): {1}", SnmpConstants.GetTypeName(pck.Pdu.VbList[0].Value.Type), pck.Pdu.VbList[0].Value.ToString());
                    NomeStampantePdu = pck.Pdu.VbList[0].Value.ToString();
                    return NomeStampantePdu;
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }
            return NomeStampantePdu;
        }

        private void SalvataggioDatiDB(SnmpV1Packet pck)                                //Analizza il pdu e carica il valore del dato richiesto su db
        {
            if (pck != null)
            {
                if (pck.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", pck.Pdu.ErrorStatus, pck.Pdu.ErrorIndex);
                }
                else
                {
                    UpdateDB(pck);
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }

        }

        private void LeggiDatiStampante(UdpTarget target, AgentParameters param, Entities SNMPContext)
        {
            Pdu pdu = new Pdu(PduType.Get);                                 //Unità dati dello scambio messaggi  
            pdu.VbList.Add(".1.3.6.1.2.1.1.1.0");                           //Nome stampante
            SnmpV1Packet auth = (SnmpV1Packet)target.Request(pdu, param);
            String nomeStampante = GetNomeStampante(auth);
            var Allvendor = SNMPContext.OIDs.Select(x => x.Vendor);         //query per ottenere tutti i nomi dei vendor conosciuti
            var vendor = Allvendor.Distinct();                              //distinct per eliminare i doppioni dall'elenco vendor

            foreach (var v in vendor)
            {
                if (nomeStampante.Contains(v.ToString()))
                {
                    string produttore = v.ToString();
                    Console.WriteLine("Produttore Stampanti supportato ==> " + v.ToString());

                    var item = SNMPContext.OIDs.Where(x => x.Vendor == produttore);
                    foreach (var i in item)
                    {
                        pdu.VbList.Add(i.NumeroOID);
                    }

                    SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP 
                    SalvataggioDatiDB(result);
                }
                else
                {
                    Console.WriteLine("Produttore Stampanti non supportato");

                }
            }
        }

        private void CheckStampanteBotton_Click(object sender, EventArgs e)
        {
            LeggiDatiStampante(target,param, SNMPContext);
        }
        
        public void Log(String ip, String disp)                                       //costruzione messaggio del log
        {
            StreamWriter w = File.AppendText(@"C:\Users\fabia\Desktop\Log.txt");
            w.Write("Dispositivo Trovato: ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("->Nome: {0}, IP:{1}", disp, ip);
            w.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
            w.Flush();
            w.Close();
        }

        private async void discoverBotton_ClickAsync(object sender, EventArgs e)             //controlla la rete locale per individuare le stampanti. Sono individuate tramite risposta al broadcast
        {
            Discoverer discoverer = new Discoverer();
            discoverer.AgentFound += DiscovererAgentFound;
            Console.WriteLine("v1 discovery");
            await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, 161), new Lextm.SharpSnmpLib.OctetString("public"), 6000);
        }

        void DiscovererAgentFound(object sender, AgentFoundEventArgs e)
        {
            Log(e.Agent.ToString(), e.Variable.Data.ToString()); //Dispositivo trovato nella rete e scritto nel file di log
            SalvaDatiDiscovery(e.Agent.ToString(), e.Variable.Data.ToString());
        }       
    }
}


