using System;
using System.Collections.Generic;

namespace Sigeret.Models
{
    public partial class ModeloEquipo
    {
        public ModeloEquipo()
        {
            this.Equipoes = new List<Equipo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public string Modelo { get; set; }
        public virtual ICollection<Equipo> Equipoes { get; set; }
    }
}
