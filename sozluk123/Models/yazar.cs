
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace sozluk123.Models
{

using System;
    using System.Collections.Generic;
    
public partial class yazar
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public yazar()
    {

        this.active = true;

        this.baslik = new HashSet<baslik>();

        this.entry = new HashSet<entry>();

        this.entry_yazar = new HashSet<entry_yazar>();

    }


    public System.Guid ID { get; set; }

    public string yazar_ismi { get; set; }

    public string yazar_yorum { get; set; }

    public Nullable<System.DateTime> kayit_tarih { get; set; }

    public Nullable<bool> active { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<baslik> baslik { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<entry> entry { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<entry_yazar> entry_yazar { get; set; }

}

}
