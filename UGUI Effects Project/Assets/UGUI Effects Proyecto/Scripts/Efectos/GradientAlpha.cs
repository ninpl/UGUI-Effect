//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// GradientAlpha.cs (06/06/2017)												\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Efecto Gradient Alpha para la UI							\\
// Fecha Mod:		06/06/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#endregion

/*	NOTAS	->
	Degradado del alpha
*/

namespace MoonAntonio.UIEffect
{
	/// <summary>
	/// <para>Efecto Gradient Alpha para la UI</para>
	/// </summary>
	[AddComponentMenu("UI/Effects/Gradient Alpha"), RequireComponent(typeof(Graphic))]
	public class GradientAlpha : BaseMeshEffect
	{
		#region Constantes
		/// <summary>
		/// <para>Vertices de la textura</para>
		/// </summary>
		private const int TextVert = 6;										// Vertices de la textura
		#endregion

		#region Variables Privadas
		/// <summary>
		/// <para>Alpha en el top</para>
		/// </summary>
		[SerializeField, Range(0f, 1f)]
		private float alphaTop = 1f;										// Alpha en el top
		/// <summary>
		/// <para>Alpha en el bot</para>
		/// </summary>
		[SerializeField, Range(0f, 1f)]
		private float alphaBottom = 1f;										// Alpha en el bot
		/// <summary>
		/// <para>Alpha en el Left</para>
		/// </summary>
		[SerializeField, Range(0f, 1f)]
		private float alphaLeft = 1f;										// Alpha en el Left
		/// <summary>
		/// <para>Alpha en el Right</para>
		/// </summary>
		[SerializeField, Range(0f, 1f)]
		private float alphaRight = 1f;										// Alpha en el Right
		/// <summary>
		/// <para>OffSet degradado Vertical</para>
		/// </summary>
		[SerializeField, Range(-1f, 1f)]
		private float gradientOffsetVertical = 0f;							// OffSet degradado Vertical
		/// <summary>
		/// <para>OffSet degradado Horizontal</para>
		/// </summary>
		[SerializeField, Range(-1f, 1f)]
		private float gradientOffsetHorizontal = 0f;						// OffSet degradado Horizontal
		/// <summary>
		/// <para>Separacion de textura</para>
		/// </summary>
		[SerializeField]
		private bool splitTextGradient = false;								// Separacion de textura
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Alpha Top</para>
		/// </summary>
		public float AlphaTop
		{
			get { return alphaTop; }
			set { if (alphaTop != value) { alphaTop = Mathf.Clamp01(value); Actualizar(); } }
		}
		/// <summary>
		/// <para>Alpha Bot</para>
		/// </summary>
		public float AlphaBottom
		{
			get { return alphaBottom; }
			set { if (alphaBottom != value) { alphaBottom = Mathf.Clamp01(value); Actualizar(); } }
		}
		/// <summary>
		/// <para>Alpha Left</para>
		/// </summary>
		public float AlphaLeft
		{
			get { return alphaLeft; }
			set { if (alphaLeft != value) { alphaLeft = Mathf.Clamp01(value); Actualizar(); } }
		}
		/// <summary>
		/// <para>Alpha Right</para>
		/// </summary>
		public float AlphaRight
		{
			get { return alphaRight; }
			set { if (alphaRight != value) { alphaRight = Mathf.Clamp01(value); Actualizar(); } }
		}
		/// <summary>
		/// <para>Degradado vertical</para>
		/// </summary>
		public float GradientOffsetVertical
		{
			get { return gradientOffsetVertical; }
			set { if (gradientOffsetVertical != value) { gradientOffsetVertical = Mathf.Clamp(value, -1f, 1f); Actualizar(); } }
		}
		/// <summary>
		/// <para>Degradado horizontal</para>
		/// </summary>
		public float GradientOffsetHorizontal
		{
			get { return gradientOffsetHorizontal; }
			set { if (gradientOffsetHorizontal != value) { gradientOffsetHorizontal = Mathf.Clamp(value, -1f, 1f); Actualizar(); } }
		}
		/// <summary>
		/// <para>Separar Textura</para>
		/// </summary>
		public bool SplitTextGradient
		{
			get { return splitTextGradient; }
			set { if (splitTextGradient != value) { splitTextGradient = value; Actualizar(); } }
		}
		#endregion

		#region Metodos
		/// <summary>
		/// <para>Metodo reservado para modificar la malla.</para>
		/// </summary>
		/// <param name="vh">Vertices</param>
		public override void ModifyMesh(VertexHelper vh)// Metodo reservado para modificar la malla.
		{
			// Comprobamos si esta activo
			if (IsActive() == false) return;

			// Asignamos las variables
			List<UIVertex> vList = UIEffectListPool<UIVertex>.Get();

			// Obtenemos los vertices
			vh.GetUIVertexStream(vList);

			// Modificamos los vertices
			ModVertices(vList);

			// Limpiamos la lista y agregamos los triangulos
			vh.Clear();
			vh.AddUIVertexTriangleStream(vList);

			// Lanzamos el efecto
			UIEffectListPool<UIVertex>.Release(vList);
		}

		/// <summary>
		/// <para>Modifica los vertices de una lista</para>
		/// </summary>
		/// <param name="vList">Lista de vertices</param>
		private void ModVertices(List<UIVertex> vList)// Modifica los vertices de una lista
		{
			// Comprobamos si esta activo
			if (IsActive() == false || vList == null || vList.Count == 0) return;

			// Asignamos las variables
			float minX = 0f, minY = 0f, maxX = 0f, maxY = 0f, width = 0f, height = 0;
			UIVertex newVertex;

			// Recorremos la lista
			for (int i = 0; i < vList.Count; i++)
			{
				if (i == 0 || (splitTextGradient && i % TextVert == 0))
				{
					minX = vList[i].position.x;
					minY = vList[i].position.y;
					maxX = vList[i].position.x;
					maxY = vList[i].position.y;

					int vertNum = splitTextGradient ? i + TextVert : vList.Count;

					for (int k = i; k < vertNum; k++)
					{
						if (k >= vList.Count)
						{
							break;
						}
						UIVertex vertex = vList[k];
						minX = Mathf.Min(minX, vertex.position.x);
						minY = Mathf.Min(minY, vertex.position.y);
						maxX = Mathf.Max(maxX, vertex.position.x);
						maxY = Mathf.Max(maxY, vertex.position.y);
					}

					width = maxX - minX;
					height = maxY - minY;
				}

				// Asignamos las variables
				newVertex = vList[i];
				float alphaOriginal = newVertex.color.a / 255f;
				float alphaVertical = Mathf.Lerp(alphaBottom, alphaTop, (height > 0 ? (newVertex.position.y - minY) / height : 0) + gradientOffsetVertical);
				float alphaHorizontal = Mathf.Lerp(alphaLeft, alphaRight, (width > 0 ? (newVertex.position.x - minX) / width : 0) + gradientOffsetHorizontal);
				newVertex.color.a = (byte)(Mathf.Clamp01(alphaOriginal * alphaVertical * alphaHorizontal) * 255);

				// Agregamos
				vList[i] = newVertex;
			}
		}

		/// <summary>
		/// <para>Actualizamos los graficos</para>
		/// </summary>
		private void Actualizar()// Actualizamos los graficos
		{
			if (graphic != null)
			{
				graphic.SetVerticesDirty();
			}
		}
		#endregion
	}
}
