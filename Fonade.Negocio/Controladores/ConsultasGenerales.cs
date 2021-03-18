using Datos;
using Fonade.DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Controladores
{
	public class ConsultasGenerales
	{
		private string _Cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
		public int getIdTipoCampo(int _idCampo)
		{
			int tipoCampo = 0;

			using (FonadeDBDataContext db = new FonadeDBDataContext(_Cadena))
			{
				tipoCampo = (int)(from C in db.Campos
								  where C.id_Campo == _idCampo
								  select C.TipoCampo).FirstOrDefault();
			}

			return tipoCampo;
		}

		public List<StructTipoCampoValores> getPuntajes(int _idCampo)
		{
			List<StructTipoCampoValores> modelTipoCampoValores = new List<StructTipoCampoValores>();

			using (FonadeDBDataContext db = new FonadeDBDataContext(_Cadena))
			{
				modelTipoCampoValores = (from T in db.TipoCampoValores
										 where T.id_Campo == _idCampo
										 orderby T.Puntajes
										 select new StructTipoCampoValores
										 {
											 id = T.idTipoCampoVal,
											 id_campo = T.id_Campo,
											 puntaje = T.Puntajes
										 }).ToList();
			}

			return modelTipoCampoValores;
		}



	}


}
