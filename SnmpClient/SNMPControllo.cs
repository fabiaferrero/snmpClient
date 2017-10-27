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
        private int ControllaStampanti(string IndirizzoMAC, string nome, string ip)       //controlla se nella tabella stampanti vi  già la stampante scoperta
        {
            int fk = 0;
            var recordStampante = SNMPContext.Stampantis.Where(x => x.MAC == IndirizzoMAC);
            if (recordStampante.Count() == 0)
            {
                Stampanti NuovaStampante = new Stampanti
                {
                    IDStampante = CreaId(SNMPContext.Stampantis.Select(x => x.IDStampante)),
                    Nome = nome,
                    IP = ip,
                    MAC = IndirizzoMAC,
                    Data = System.DateTime.Now
                };
                SNMPContext.Stampantis.Add(NuovaStampante);
                SNMPContext.SaveChanges();
            }
            else
            {
                foreach (var riga in recordStampante)
                {
                    fk = riga.IDStampante;
                    riga.Data = DateTime.Now;
                }
                SNMPContext.SaveChanges();
            }
            //Console.WriteLine("CHIAVE PRIMARIA"+fk);
            return fk;
        }
        private string RichiediMac(string IP, string oid_mac)
        {
            string mac = System.String.Empty;
            Pdu pdu = new Pdu(PduType.Get);                                 //Unità dati dello scambio messaggi  
            pdu.VbList.Add(oid_mac);                           //mac stampante
            Console.WriteLine("OID RICHIESTO" + oid_mac);
            SnmpV1Packet PckMac = (SnmpV1Packet)target.Request(pdu, param);
            mac = GetDatoStampante(PckMac);
            return mac;
        }
        private void SalvaDatiDiscovery(string ip, string nome)
        {
            string IndMac = System.String.Empty;
            string produttore = System.String.Empty;
            string oid_mac = System.String.Empty;
            int ChiaveStampante;
            var Allvendor = SNMPContext.OIDs.Select(x => x.Vendor);         //query per ottenere tutti i nomi dei vendor conosciuti
            var vendor = Allvendor.Distinct();                              //distinct per eliminare i doppioni dall'elenco vendor

            foreach (var v in vendor)
            {
                if (nome.Contains(v.ToString()))
                {
                    produttore = v.ToString();
                    var item = SNMPContext.OIDs.Where(x => x.Vendor == produttore);
                    foreach (var i in item)
                    {
                        if (i.TipoDato.Equals("MAC"))
                            oid_mac = i.NumeroOID;
                        //Console.WriteLine("MAC Richiesto"+produttore);
                    }
                }
            }

            IndMac = RichiediMac(ip, oid_mac);
            ChiaveStampante = ControllaStampanti(IndMac, nome, ip);

            Discovery DispositivoScoperto = new Discovery
            {
                IDDiscovery =CreaId(SNMPContext.Discoveries.Select(x=>x.IDDiscovery)),
                IDStampante =ChiaveStampante,
                MAC =IndMac,
                IP =ip,
                Nome =nome,
                Data = System.DateTime.Now
            };

        SNMPContext.Discoveries.Add(DispositivoScoperto);
        SNMPContext.SaveChanges();
            
        }

        private string TrovaTipoDato(IQueryable<string> t)
        {
            string Tipo = System.String.Empty;
            
            return Tipo;
        }

        private void UpdateDB(SnmpV1Packet pck)
        {
            foreach (var i in pck.Pdu.VbList)
            {
                var listaOid = SNMPContext.OIDs.Where(x => x.NumeroOID.Equals(i.Oid));
                foreach(var Myoid in listaOid)
                {
                    ValoriStampanti risultatoSNMP = new ValoriStampanti //salvataggio dati richiesti su database
                    {
                        IDValore = CreaId(SNMPContext.ValoriStampantis.Select(x => x.IDValore)),
                        IDDiscovery = 0,
                        TipoDato = "C",
                        Valore = i.Value.ToString(),
                        Data = DateTime.Now
                    };
                    SNMPContext.ValoriStampantis.Add(risultatoSNMP);  
                }
                SNMPContext.SaveChanges();
            }
        }

        private string GetDatoStampante(SnmpV1Packet pck)                                    //Analizza il pdu per ottenere il nome della stampante per verificare supporto
        {
            string StampantePdu = System.String.Empty;
            if (pck != null)
            {
                if (pck.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", pck.Pdu.ErrorStatus, pck.Pdu.ErrorIndex);
                }
                else
                {
                    Console.WriteLine("NomeStampantePdu: ({0}): {1}", SnmpConstants.GetTypeName(pck.Pdu.VbList[0].Value.Type), pck.Pdu.VbList[0].Value.ToString());
                    StampantePdu = pck.Pdu.VbList[0].Value.ToString();
                    return StampantePdu;
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }
            return StampantePdu;
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
            String nomeStampante = GetDatoStampante(auth);
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
                        Console.WriteLine("OID da richiedere"+i.TipoDato);
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
            Console.WriteLine("Discovery");
            await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, 161), new Lextm.SharpSnmpLib.OctetString("public"), 6000);
        }

        void DiscovererAgentFound(object sender, AgentFoundEventArgs e)
        {
            Log(e.Agent.ToString(), e.Variable.Data.ToString()); //Dispositivo trovato nella rete e scritto nel file di log
            SalvaDatiDiscovery(e.Agent.ToString(), e.Variable.Data.ToString());
        }       
    }
}


