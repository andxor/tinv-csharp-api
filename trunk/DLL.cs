/*!
 * TInv Services C# Interface 
 * (c) 2020 Luca Palmulli <l.palmulli@andxor.it>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tinv_csharp_api
{
    public class userService
    {
        private readonly userServices.GatewayServiziUtente_service srv;

        public userService()
        {
            srv = new userServices.GatewayServiziUtente_service();
        }

        public userService(string url)
        {
            srv = new userServices.GatewayServiziUtente_service();
            srv.Url = "https://" + url + "/userServices";
        }

        public userServices.AutenticazioneType auth(String idPaese, String idCodice, String Password, String paeseGestito, String codiceGestito)
        {
            userServices.AutenticazioneType a = new userServices.AutenticazioneType();
            userServices.IdFiscaleType id = new userServices.IdFiscaleType();
            userServices.IdFiscaleType idGest = new userServices.IdFiscaleType();

            id.IdPaese = idPaese;
            id.IdCodice = idCodice;
            a.Cedente = id;
            a.Password = Password;
            if (paeseGestito != null)
            {
                idGest.IdPaese = paeseGestito;
                if (codiceGestito != null)
                {
                    idGest.IdCodice = codiceGestito;
                    a.Gestione = idGest;
                }
            }
            return a;
        }


        /**
         * Need to define manually :
         * 
         * 
         * 
         *          DECLARATIONS
         *          private bool dataInizioFieldSpecified;
         *          private bool dataFineFieldSpecified;
         * 
         *          METHODS
                    /// <remarks/>
                    [System.Xml.Serialization.XmlIgnoreAttribute()]
                    public bool DataInizioSpecified
                    {
                        get
                        {
                            return this.dataInizioFieldSpecified;
                        }
                        set
                        {
                            this.dataInizioFieldSpecified = value;
                        }
                    }

                    /// <remarks/>
                    [System.Xml.Serialization.XmlIgnoreAttribute()]
                    public bool DataFineSpecified
                    {
                        get
                        {
                            return this.dataFineFieldSpecified;
                        }
                        set
                        {
                            this.dataFineFieldSpecified = value;
                        }
                    }
         * 
         * 
         * 
         *  */

        public userServices.FilterType setFilter(userServices.AutenticazioneType auth, String testo, String dataInizio, String dataFine, String limite)
        {
            Console.WriteLine("Setting up Filter...");
            userServices.FilterType filter = new userServices.FilterType();
            filter.Autenticazione = auth;
            if (testo != null)
            {
                filter.Testo = testo;
            }
            if (limite != null)
            {
                try
                {
                    filter.LimiteSpecified = true;
                    filter.Limite = int.Parse(limite);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Valore limite non riconosciuto: " + e);
                    throw;
                }

            }
            try
            {
                if (dataInizio != null)
                {
                    filter.DataInizioSpecified = true;
                    try
                    {
                        //filter.DataInizio = DateTime.ParseExact(dataInizio, "yyyy-MM-dd", null).Date;
                        filter.Item = DateTime.ParseExact(dataInizio, "yyyy-MM-dd", null).Date;
                        filter.ItemElementName = userServices.ItemChoiceType2.DataInizio;
                    }
                    catch (Exception)
                    {
                        //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        //filter.DataInizio = new DateTime(long.Parse(dataInizio));//start.AddMilliseconds(long.Parse(dataFine)).ToLocalTime();                   
                        filter.Item = new DateTime(long.Parse(dataInizio));
                        filter.ItemElementName = userServices.ItemChoiceType2.DataOraInizio;
                    }
                }
                if (dataFine != null)
                {
                    filter.DataFineSpecified = true;
                    try
                    {
                        filter.Item1 = DateTime.ParseExact(dataFine, "yyyy-MM-dd", null).Date;
                        filter.Item1ElementName = userServices.Item1ChoiceType.DataFine;
                    }
                    catch (Exception)
                    {
                        //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        filter.Item1 = new DateTime(long.Parse(dataFine));//start.AddMilliseconds(long.Parse(dataFine)).ToLocalTime();     
                        filter.Item1ElementName = userServices.Item1ChoiceType.DataOraFine;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Data non riconosciuta." + e);
                throw;
            }
            return filter;
        }

        public userServices.ShortStatusType[] elencoFatture(userServices.FilterType filter)  //setup filter with setFilter function
        {
            Console.WriteLine("Elenco Fatture");
            userServices.ShortStatusType[] fatture;
            try
            {
                fatture = srv.ElencoFatture(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                //fatture = new userServices.ShortStatusType[0];
                throw e;
            }
            return fatture;
        }

        public userServices.PasvFilterType setPasvFilter(userServices.AutenticazioneType auth, String testo, String dataInizio, String dataFine, String limite, bool includiArchiviate)
        {
            Console.WriteLine("Setting up PasvFilter...");
            userServices.PasvFilterType filter = new userServices.PasvFilterType();
            filter.Autenticazione = auth;
            if (testo != null)
            {
                filter.Testo = testo;
            }
            if (limite != null)
            {
                try
                {
                    filter.LimiteSpecified = true;
                    filter.Limite = int.Parse(limite);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Valore limite non riconosciuto: " + e);
                    throw;
                }

            }
            try
            {
                if (dataInizio != null)
                {
                    filter.DataInizioSpecified = true;
                    try
                    {
                        filter.Item = DateTime.ParseExact(dataInizio, "yyyy-MM-dd", null).Date;
                        filter.ItemElementName = userServices.ItemChoiceType3.DataInizio;
                    }
                    catch (Exception)
                    {
                        //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        //filter.DataInizio = new DateTime(long.Parse(dataInizio));//start.AddMilliseconds(long.Parse(dataFine)).ToLocalTime();
                        filter.Item = new DateTime(long.Parse(dataInizio));
                        filter.ItemElementName = userServices.ItemChoiceType3.DataOraInizio;
                    }
                }
                if (dataFine != null)
                {
                    filter.DataFineSpecified = true;
                    try
                    {
                        //filter.DataFine = DateTime.ParseExact(dataFine, "yyyy-MM-dd", null).Date;
                        filter.Item1 = DateTime.ParseExact(dataFine, "yyyy-MM-dd", null).Date;
                        filter.Item1ElementName = userServices.Item1ChoiceType1.DataFine;
                    }
                    catch (Exception)
                    {

                        //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        //filter.DataFine = new DateTime(long.Parse(dataFine));//start.AddMilliseconds(long.Parse(dataFine)).ToLocalTime();
                        filter.Item1 = new DateTime(long.Parse(dataFine));//start.AddMilliseconds(long.Parse(dataFine)).ToLocalTime();     
                        filter.Item1ElementName = userServices.Item1ChoiceType1.DataOraFine;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Data non riconosciuta." + e);
                throw;
            }
            filter.IncludiArchiviate = includiArchiviate;
            return filter;
        }

        public userServices.PasvShortStatusType[] elencoFatturePassive(userServices.PasvFilterType pasvfilter)  //setup filter with setPasvFilter function
        {
            Console.WriteLine("Elenco Fatture passive");
            userServices.PasvShortStatusType[] fatture;
            try
            {
                fatture = srv.PasvElencoFatture(pasvfilter);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                //fatture = new userServices.PasvShortStatusType[0];
                throw;
            }
            return fatture;
        }

        public userServices.QueryType setQuery(userServices.AutenticazioneType auth, String progr, String progrRic, String minimal)
        {
            userServices.QueryType q = new userServices.QueryType();
            q.Autenticazione = auth;
            q.ProgressivoInvio = progr;
            if (progrRic != null)
            {
                q.ProgressivoRicezione = progrRic;
            }
            if (minimal != null)
            {
                try
                {
                    bool min = Boolean.Parse(minimal);
                    q.MinimalSpecified = true;
                    q.Minimal = min;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Booleano non riconosciuto." + e);
                    throw;
                }
            }
            return q;
        }

        public userServices.FileType download(userServices.QueryType query)
        {
            Console.WriteLine("Download");
            userServices.FileType xml;
            try
            {
                xml = srv.Download(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                throw;
            }
            return xml;
        }

        public userServices.PasvQueryType setPasvQuery(userServices.AutenticazioneType auth, String idSdi, String pos, String minimal, bool unwrap)
        {
            Console.WriteLine("Setting up Query");
            userServices.PasvQueryType q = new userServices.PasvQueryType();
            q.Autenticazione = auth;
            q.IdentificativoSdI = idSdi;
            q.Unwrap = unwrap;
            try
            {
                if (minimal != null)
                {
                    bool min = Boolean.Parse(minimal);
                    q.MinimalSpecified = true;
                    q.Minimal = min;
                }
                if (pos != null)
                {
                    UInt16 position = UInt16.Parse(pos);
                    q.PosizioneSpecified = true;
                    q.Posizione = position;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Booleano non riconosciuto." + e);
                throw;
            }
            return q;
        }


        public userServices.FileType downloadPassive(userServices.PasvQueryType query)
        {
            Console.WriteLine("Download Passive");
            userServices.FileType xml;
            try
            {
                xml = srv.PasvDownload(query);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                throw;
            }
            return xml;
        }

        public userServices.SimpleQueryType setSimpleQuery(userServices.AutenticazioneType auth, String progr)
        {
            userServices.SimpleQueryType simpleq = new userServices.SimpleQueryType();
            simpleq.Autenticazione = auth;
            simpleq.ProgressivoInvio = progr;
            return simpleq;
        }


        public userServices.FullStatusType statoFattura(userServices.SimpleQueryType simpleq)
        {
            Console.WriteLine("Stato fattura");
            userServices.FullStatusType status;
            try
            {
                status = srv.Stato(simpleq);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                throw;
            }
            return status;
        }


        /** Need to define manually choices for Item1, Item2, Item3 :
         *          DECLARATIONS 
         *          
         *          private ItemChoiceType5 itemElementNameField1;

                    private ItemChoiceType6 itemElementNameField2;

                    private ItemChoiceType7 itemElementNameField3; 
         
                    
                    CHOICE IDENTIFIER

                    [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        
            
                    CHOICE METHOD
                    /// <remarks/>
                    [System.Xml.Serialization.XmlIgnoreAttribute()]
                    public ItemChoiceType ItemElementName {
                        get {
                            return this.itemElementNameField;
                        }
                        set {
                            this.itemElementNameField = value;
                        }
                    }
             
                    CHOICE DEFINITION
                    /// <remarks/>
                    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
                    [System.SerializableAttribute()]
                    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.andxor.com/fatturapa/wsdl", IncludeInSchema=false)]
                    public enum ItemChoiceType {
        
                        /// <remarks/>
                        [System.Xml.Serialization.XmlEnumAttribute(":CodiceDestinatario")]
                        CodiceDestinatario,
        
                        /// <remarks/>
                        [System.Xml.Serialization.XmlEnumAttribute(":PECDestinatario")]
                        PECDestinatario,
                    }     
                   
             *
             *
             */

        public userServices.RicevutaType inviaFatturaStr(userServices.AutenticazioneType auth, String codiceDestinatario, String pecDestinatario, String overrideCedente, bool terzoIntermediario, String cessionarioCommittente, String fatturaElettronicaBody)
        {
            Console.WriteLine("Invia fattura (string version)");
            userServices.FatturaType f = new userServices.FatturaType();
            f.Autenticazione = auth;
            if (codiceDestinatario != null && pecDestinatario == null)
            {
                f.Item = codiceDestinatario;
                f.ItemElementName = userServices.ItemChoiceType.CodiceDestinatario;
            }
            else if (pecDestinatario != null && codiceDestinatario == null)
            {
                f.Item = pecDestinatario;
                f.ItemElementName = userServices.ItemChoiceType.PECDestinatario;
            }
            else
            {
                Console.WriteLine("Errore, inseriti sia CodiceDestinatario che PECDestinatario");
                throw new Exception("Non é possibile valorizzare sia CodiceDestinatario che PECDestinatario");
            }
            if (overrideCedente != null)
            {
                Console.WriteLine("Attenzione: overrideCedente specificato.");
                f.Item1 = overrideCedente;
                f.ItemElementName1 = userServices.ItemChoiceType5.OverrideCedenteStr;
            }
            if (cessionarioCommittente != null)
            {
                f.Item2 = cessionarioCommittente;
                f.ItemElementName2 = userServices.ItemChoiceType6.CessionarioCommittenteStr;
            }
            if (fatturaElettronicaBody != null)
            {
                f.Item3 = fatturaElettronicaBody;
                f.ItemElementName3 = userServices.ItemChoiceType7.FatturaElettronicaBodyStr;
            }
            if (terzoIntermediario)
            {
                f.TerzoIntermediarioSpecified = true;
                f.TerzoIntermediario = true;
            }
            try
            {
                return srv.InviaFattura(f);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                throw;
            }
        }

        public userServices.AttNotifierType scaricaNotificaAttiva(userServices.AutenticazioneType auth, String idSdi)
        {
            Console.WriteLine("Download notifica attiva in corso...");
            userServices.NotifyFEType req = new userServices.NotifyFEType();
            req.Autenticazione = auth;
            req.IdentificativoSdI = idSdi;
            try
            {
                return srv.AttNotificaFE(req);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                throw;
            }
        }

        public userServices.NotifierType scaricaNotificaPassiva(userServices.AutenticazioneType auth, String idSdi)
        {
            Console.WriteLine("Download notifica passiva in corso...");
            userServices.PasvNotifyFEType req = new userServices.PasvNotifyFEType();
            req.Autenticazione = auth;
            req.IdentificativoSdI = idSdi;
            try
            {
                return srv.PasvNotificaFE(req);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                throw;
            }
        }

        public userServices.ZipQueryType setZipQuery(userServices.AutenticazioneType auth, String progr)
        {
            userServices.ZipQueryType z = new userServices.ZipQueryType();
            z.Autenticazione = auth;
            if (progr != null)
            {
                z.ProgressivoInvio = progr;
                return z;
            }
            else
            {
                throw new Exception("Nessun progressivo inserito.");
            }
        }

        public userServices.FileType downloadZipInvoice(userServices.AutenticazioneType auth, userServices.ZipQueryType zq)
        {
            userServices.FileType zip = new userServices.FileType();
            try
            {
                zip = srv.DownloadZip(zq);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                throw;
            }
            return zip;
        }

        public userServices.PasvZipQueryType setPasvZipQuery(userServices.AutenticazioneType auth, String idSdi)
        {
            userServices.PasvZipQueryType z = new userServices.PasvZipQueryType();
            z.Autenticazione = auth;
            if (idSdi != null)
            {
                z.IdentificativoSdI = idSdi;
                return z;
            }
            else
            {
                throw new Exception("Nessun identificativo SdI inserito.");
            }
        }

        public userServices.FileType downloadZipPassiveInvoice(userServices.AutenticazioneType auth, userServices.PasvZipQueryType zq)
        {
            userServices.FileType zip = new userServices.FileType();
            try
            {
                zip = srv.PasvDownloadZip(zq);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                throw;
            }
            return zip;
        }
    }
}
