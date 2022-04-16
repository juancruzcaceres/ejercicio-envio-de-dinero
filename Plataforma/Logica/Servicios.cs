using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Servicios
    {

        
        public Movimiento CrearMovimiento(User emisor, User destinatario, string descripcion, decimal monto)
        {
            Movimiento movimiento = new Movimiento() { ID = (Principal.Instance.Movimientos().Count) + 1, Emisor=emisor, Destinatario=destinatario ,Fecha = DateTime.Now, Descripcion = descripcion, Monto = monto };
            return movimiento;
        }


        //revisar returns (hay que retornar 201 y error 400)
        public int CrearMovimientoEnvio(string dniEmisor, string dniDestinatario, string descripcion, decimal monto)
        {
            
            User emisor = Principal.Instance.BuscarUsuarioPorDNI(dniEmisor);
            User destinatario = Principal.Instance.BuscarUsuarioPorDNI(dniDestinatario);

            if (Principal.Instance.Usuarios().Contains(emisor) && Principal.Instance.Usuarios().Contains(destinatario))
            {
                if (emisor.Saldo >= monto)
                {
                    Movimiento movimiento = CrearMovimiento(emisor, destinatario, descripcion, monto);
                    movimiento.Monto = -monto;
                    emisor.Movimientos.Add(movimiento);
                    emisor.Saldo += monto;

                    Movimiento movimiento2 = CrearMovimiento(emisor, destinatario, descripcion, monto);
                    movimiento2.Monto = monto;
                    destinatario.Movimientos.Add(movimiento2);
                    destinatario.Saldo += monto;

                    return 201;
                }
            };
            return 400;
        }

        public bool CancelarMovimiento(int idMovimiento)
        {
            Movimiento movimiento = Principal.Instance.Movimientos().Find(x => x.ID == idMovimiento);

            Movimiento nuevoMovimiento = CrearMovimiento(movimiento.Emisor, movimiento.Destinatario, "Cancelación: " + movimiento.Descripcion, movimiento.Monto);
            nuevoMovimiento.Monto = movimiento.Monto;
            nuevoMovimiento.Emisor.Movimientos.Add(nuevoMovimiento);
            nuevoMovimiento.Emisor.Saldo += nuevoMovimiento.Monto;

            Movimiento nuevoMovimiento2 = CrearMovimiento(movimiento.Emisor, movimiento.Destinatario, "Cancelación: " + movimiento.Descripcion, movimiento.Monto);
            nuevoMovimiento2.Monto = -movimiento.Monto;
            nuevoMovimiento2.Destinatario.Movimientos.Add(nuevoMovimiento2);
            nuevoMovimiento2.Destinatario.Saldo += nuevoMovimiento2.Monto;

            return true;
        }

        public List<Movimiento> ObtenerMovimientosDeUsuario(string dni)
        {
            try
            {
                List<Movimiento> movimientos = Principal.Instance.Usuarios().Find(x => x.DNI == dni).Movimientos;
                movimientos = movimientos.OrderByDescending(x => x.Fecha).ToList();
                return movimientos;
            }
            catch (Exception)
            {
                throw new Exception("404 Not found");
            }
        }

    }
}
