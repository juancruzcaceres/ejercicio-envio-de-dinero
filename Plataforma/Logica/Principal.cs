using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public sealed class Principal
    {
        private readonly static Principal _instance = new Principal();

        public static Principal Instance { get { return _instance; } }

        public static List<User> usuarios { get; set; }
        public static List<Movimiento> movimientos { get; set; }

        public User BuscarUsuarioPorDNI(string dni)
        {
            try
            {
                return usuarios.Find(x => x.DNI == dni);
            }
            catch (Exception)
            {
                throw new Exception("El usuario solicitado no existe");
            }
        }

        public List<Movimiento> Movimientos()
        {
            return movimientos;
        }

        public List<User> Usuarios()
        {
            return usuarios;
        }
    }
}
