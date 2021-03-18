using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
	public class ConsultasTablaDeEvaluacion
	{

		// Cambio para agregar varios puntajes


		private string _Cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

		//obtiene el id del tipoCampo - William P.L.
		public int getIdTipoCampo(int _idCampo)
		{
			int tipoCampo = 0;

			using (FonadeDBDataContext db = new FonadeDBDataContext(_Cadena))
			{
				tipoCampo = (int)(from C in db.Campo
								  where C.id_Campo == _idCampo
								  select C.TipoCampo).FirstOrDefault();
			}

			return tipoCampo;
		}

		//obtiene los puntajes asociados al id_campo si el tipo campo es igual a 2 - William P.L.
		public List<TipoCampoValoresModel> getPuntajes(int _idCampo)
		{
			List<TipoCampoValoresModel> modelTipoCampoValores = new List<TipoCampoValoresModel>();

			using (FonadeDBDataContext db = new FonadeDBDataContext(_Cadena))
			{
				modelTipoCampoValores = (from T in db.TipoCampoValores
										 where T.id_Campo == _idCampo
										 orderby T.Puntajes
										 select new TipoCampoValoresModel
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
