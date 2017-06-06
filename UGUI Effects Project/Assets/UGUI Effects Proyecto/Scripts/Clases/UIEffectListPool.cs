//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIEffectListPool.cs (06/06/2017)												\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Pool de la lista de los efectos UI							\\
// Fecha Mod:		06/06/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
#endregion

namespace MoonAntonio.UIEffect
{
	/// <summary>
	/// <para>Pool de la lista de los efectos UI.</para>
	/// </summary>
	public static class UIEffectListPool<T>
	{
		#region Variables
		/// <summary>
		/// <para>Pool de objetos para evitar asignaciones.</para>
		/// </summary>
		private static readonly UIEffectObjectPool<List<T>> listPool = new UIEffectObjectPool<List<T>>(null, l => l.Clear()); // Pool de objetos para evitar asignaciones
		#endregion

		#region API
		/// <summary>
		/// <para>Obtiene la lista de objetos de la pool.</para>
		/// </summary>
		/// <returns>Lista de objetos de la pool</returns>
		public static List<T> Get()// Obtiene la lista de objetos
		{
			return listPool.Get();
		}

		/// <summary>
		/// <para>Lanza la lista de la pool.</para>
		/// </summary>
		/// <param name="aRelease">Lista a lanzar</param>
		public static void Release(List<T> aRelease)// Lanza la lista de la pool
		{
			listPool.Release(aRelease);
		}
		#endregion
	}

	/// <summary>
	/// <para>Pool de la lista de los objetos con efectos.</para>
	/// </summary>
	public class UIEffectObjectPool<T> where T : new()
	{
		#region Variables Privadas
		/// <summary>
		/// <para>Cola de la pool</para>
		/// </summary>
		private readonly Stack<T> stack = new Stack<T>();							// Cola de la pool
		/// <summary>
		/// <para>Accion de Unity - Obteniendo</para>
		/// </summary>
		private readonly UnityAction<T> actionOnGet;								// Accion de Unity - Obteniendo
		/// <summary>
		/// <para>Accion de Unity - Lanzando</para>
		/// </summary>
		private readonly UnityAction<T> actionOnRelease;							// Accion de Unity - Lanzando
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Conteo todos</para>
		/// </summary>
		public int countAll
		{
			get;
			private set;
		}
		/// <summary>
		/// <para>Conteo de los activos</para>
		/// </summary>
		public int countActive
		{
			get { return countAll - countInactive; }
		}
		/// <summary>
		/// <para>Conteo de los inactivos</para>
		/// </summary>
		public int countInactive
		{
			get { return stack.Count; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// <para>Constructor de <see cref="UIEffectObjectPool"/></para>
		/// </summary>
		/// <param name="TactionOnGet"></param>
		/// <param name="TactionOnRelease"></param>
		public UIEffectObjectPool(UnityAction<T> TactionOnGet, UnityAction<T> TactionOnRelease)// Constructor de UIEffectObjectPool
		{
			actionOnGet = TactionOnGet;
			actionOnRelease = TactionOnRelease;
		}
		#endregion

		#region API
		/// <summary>
		/// <para>Obtiene la lista de objetos</para>
		/// </summary>
		/// <returns>Obtiene la cola</returns>
		public T Get()// Obtiene la lista de objetos
		{
			T element;

			if (stack.Count == 0)
			{
				element = new T();
				countAll++;
			}
			else
			{
				element = stack.Pop();
			}

			if (actionOnGet != null) actionOnGet(element);

			return element;
		}

		/// <summary>
		/// <para>Lanza la lista de la pool</para>
		/// </summary>
		/// <param name="item"></param>
		public void Release(T item)// Lanza la lista de la pool
		{
			if (stack.Count > 0 && ReferenceEquals(stack.Peek(), item)) Debug.LogError("Error Interno. Intentando destruir el objeto que ya se ha lanzado de la piscina.");

			if (actionOnRelease != null) actionOnRelease(item);

			stack.Push(item);
		}
		#endregion
	}
}
