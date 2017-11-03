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
            InitializeComponent();
            InitializeTimer();
        }

        private int counter;
        private void InitializeTimer()
        {
            counter = 0;
            timer.Interval = 1000;
            timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (counter >= 10)
            {
                timer.Enabled = false;
                discover();
                System.Threading.Thread.Sleep(3000);
                counter = 0;
                InitializeTimer();
            }
            else
            {
                counter = counter + 1;
                label.Text = "Secondi " + counter.ToString();
            }
        }


        private int CreaId(IQueryable<int> q)
        {
            int min = 0;
            if (q.Count() == 0)
                return 0;
            foreach (var i in q)
            {
                if (min < i)
                    min = i;
            }
            return min + 1;
        } //Crea ID del prossimo elemento da inserire nel DB risprendendo dall'ultimo
        private int CreaIdStampante(IQueryable<int> q)
        {
            int min = 0;
            if (q.Count() == 0)
                return 001;
            foreach (var i in q)
            {
                if (min < i)
                    min = i;
            }
            return min + 1;
        }
        private int OttieniChiaveDiscovery(string IndirizzoMAC)       //controlla se nella tabella stampanti vi  già la stampante scoperta
        {
            int dk = 0;
            var recordDiscovery = SNMPContext.Discoveries.Where(x => x.MAC == IndirizzoMAC);
            foreach (var riga in recordDiscovery)
            {
                dk = riga.IDDiscovery;
            }
            return dk;
        }
        private void ControllaStampanti(string IndirizzoMAC, string nome, string ip, ref int fk)       //controlla se nella tabella stampanti vi  già la stampante scoperta
        {
            string VendorStampante = System.String.Empty;
            var Allvendor = SNMPContext.OIDs.Select(x => x.Vendor);         //query per ottenere tutti i nomi dei vendor conosciuti
            var elencoVendor = Allvendor.Distinct();
            var recordStampante = SNMPContext.Stampantis.Where(x => x.MAC == IndirizzoMAC);
            foreach (var produttore in elencoVendor)
            {
                if (nome.Contains(produttore))
                    VendorStampante = produttore;
            }

            if (recordStampante.Count() == 0)
            {
                Stampanti NuovaStampante = new Stampanti
                {
                    IDStampante = fk = CreaIdStampante(SNMPContext.Stampantis.Select(x => x.IDStampante)),
                    Nome = nome,
                    IP = ip,
                    MAC = IndirizzoMAC,
                    Data = System.DateTime.Now,
                    Vendor = VendorStampante,
                };
                Console.WriteLine("mac" + IndirizzoMAC);
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

        }
        private string RichiediMac(string oid_mac/* IpAddress addrIp*/)
        {
            string mac = System.String.Empty;
            Pdu pdu = new Pdu(PduType.Get);                                 //Unità dati dello scambio messaggi  
            pdu.VbList.Add(oid_mac);                                        //mac stampante                                                                                                                                                                                                                                                                                                                                                                                                                  
            SnmpV1Packet PckMac = (SnmpV1Packet)target.Request(pdu, param);
            mac = GetDatoStampante(PckMac);
            return mac;
        }
        private void SalvaDatiDiscovery(string ip, string nome)
        {
            string IndMac = System.String.Empty;
            string produttore = System.String.Empty;
            string oid_mac = System.String.Empty;
            int ChiaveStampante = 0;
            var Allvendor = SNMPContext.OIDs.Select(x => x.Vendor);         //query per ottenere tutti i nomi dei vendor conosciuti
            var vendor = Allvendor.Distinct();


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
                    }
                }
            }

            IndMac = RichiediMac(oid_mac);
            ControllaStampanti(IndMac, nome, ip, ref ChiaveStampante);
            Discovery DispositivoScoperto = new Discovery
            {
                IDDiscovery = CreaId(SNMPContext.Discoveries.Select(x => x.IDDiscovery)),
                IDStampante = ChiaveStampante,
                MAC = IndMac,
                IP = ip,
                Nome = nome,
                Data = System.DateTime.Now
            };

            SNMPContext.Discoveries.Add(DispositivoScoperto);
            SNMPContext.SaveChanges();

        }
        private string TrovaTipoDato(string oidPdu, string produttore)
        {
            string Tipo = System.String.Empty;
            var listaOid = SNMPContext.OIDs.Where(x => x.NumeroOID.Equals(oidPdu));
            var listaOidProduttore = listaOid.Where(x => x.Vendor == produttore);
            foreach (var valore in listaOid)
            {
                Tipo = valore.TipoDato;
            }
            return Tipo;
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
                    //Console.WriteLine("NomeStampantePdu: ({0}): {1}", SnmpConstants.GetTypeName(pck.Pdu.VbList[0].Value.Type), pck.Pdu.VbList[0].Value.ToString());
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
        private void SalvataggioDatiStampantiDB(SnmpV1Packet pck, string produttore)                                //Analizza il pdu e carica il valore del dato richiesto su db
        {
            Entities contextSalvataggio = new Entities();
            if (pck != null)
            {
                if (pck.Pdu.ErrorStatus != 0)
                {
                    Console.WriteLine("Errore durante la richiesta SNMP. Errore {0} index {1}", pck.Pdu.ErrorStatus, pck.Pdu.ErrorIndex);
                }
                else
                {
                    foreach (var i in pck.Pdu.VbList)
                    {
                        string OIDmac = System.String.Empty;
                        var item = SNMPContext.OIDs.Where(x => x.Vendor == produttore);
                        foreach (var riga in item)
                        {
                            if (riga.TipoDato.Equals("MAC"))
                                OIDmac = riga.NumeroOID;
                        }
                        string IndMac = RichiediMac(OIDmac);
                        int chiaveDiscovery = OttieniChiaveDiscovery(IndMac);

                        ValoriStampanti risultatoSNMP = new ValoriStampanti //salvataggio dati richiesti su database
                        {
                            IDValore = CreaId(SNMPContext.ValoriStampantis.Select(x => x.IDValore)),
                            IDDiscovery = chiaveDiscovery,
                            TipoDato = TrovaTipoDato(i.Oid.ToString(), produttore),
                            Valore = i.Value.ToString(),
                            Data = DateTime.Now
                        };
                        contextSalvataggio.ValoriStampantis.Add(risultatoSNMP);
                        contextSalvataggio.SaveChanges();
                    }
                }
            }
            else
            {
                Console.WriteLine("Nessuna risposta dall'agente SNMP");
            }
        }

        private void CaricaOidPdu(UdpTarget target, AgentParameters param, Entities SNMPContext)
        {
            Pdu pdu = new Pdu(PduType.Get);                                 //Unità dati dello scambio messaggi  
            pdu.VbList.Add(".1.3.6.1.2.1.1.1.0");                           //Nome stampante
            SnmpV1Packet auth = (SnmpV1Packet)target.Request(pdu, param);
            String nomeStampante = GetDatoStampante(auth);
            var AllvendorOID = SNMPContext.OIDs.Select(x => x.Vendor);         //query per ottenere tutti i nomi dei vendor conosciuti
            var vendorOID = AllvendorOID.Distinct();                              //distinct per eliminare i doppioni dall'elenco vendor
            var AllVendorstampante = SNMPContext.Stampantis.Select(x => x.Vendor);
            var vendorStampante = AllVendorstampante.Distinct();
            
           
            foreach (var v in vendorStampante)
            {
                if (nomeStampante.Contains(v.ToString()))
                {
                    pdu = new Pdu(PduType.Get);
                    string produttore = v.ToString();
                    var item = SNMPContext.OIDs.Where(x => x.Vendor == produttore);
                    foreach (var i in item)
                    {
                        pdu.VbList.Add(i.NumeroOID);
                        //Console.WriteLine("OID aggiunto in pdu "+i.TipoDato+" -> "+ i.NumeroOID);
                    }
                    SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);                 //Richiesta SNMP 
                    SalvataggioDatiStampantiDB(result, produttore);
                }
                else
                {
                    Console.WriteLine("Produttore Stampanti non supportato");
                }
            }
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
        private async void discover()             //controlla la rete locale per individuare le stampanti. Sono individuate tramite risposta al broadcast
        {
            Discoverer discoverer = new Discoverer();
            discoverer.AgentFound += DiscovererAgentFound;
            Console.WriteLine("Discovery");
            await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, 161), new Lextm.SharpSnmpLib.OctetString("public"), 6000);
        }
        void DiscovererAgentFound(object sender, AgentFoundEventArgs e)
        {
            community = new SnmpSharpNet.OctetString("public");              //SNMP community name, di default "public"
            param = new AgentParameters(community);                          //Definisce parametri dell'agente, secondo community name
            param.Version = SnmpVersion.Ver1;                                //Definizione versione SNMP 1
            agent = new IpAddress(e.Agent.Address);                           //Definizione indirizzo IP della macchina
            target = new UdpTarget((IPAddress)agent, 161, 2000, 1);
            Log(e.Agent.ToString(), e.Variable.Data.ToString()); //Dispositivo trovato nella rete e scritto nel file di log
            SalvaDatiDiscovery(e.Agent.ToString(), e.Variable.Data.ToString());
            CaricaOidPdu(target, param, SNMPContext);
        }
    }
}


     

    
