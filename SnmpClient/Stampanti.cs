
//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SnmpClient
{

using System;
    using System.Collections.Generic;
    
public partial class Stampanti
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Stampanti()
    {

        this.ValoriStampantis = new HashSet<ValoriStampanti>();

    }


    public int IDStampante { get; set; }

    public string Nome { get; set; }

    public string IP { get; set; }

    public System.DateTime Data { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ValoriStampanti> ValoriStampantis { get; set; }

}

}
