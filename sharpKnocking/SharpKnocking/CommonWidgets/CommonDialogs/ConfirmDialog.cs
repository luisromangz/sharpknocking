
using System;
using Gtk;

namespace SharpKnocking.Common.Widgets.CommonDialogs
{	
	/// <summary>
	/// This class specialices <c>MessageDialog</c>
	/// so it always show a "Yes/No" question dialog.
	/// </summary>
	public class ConfirmDialog : MessageDialog
	{
		/// <summary>
		/// <c>ConfirmDialog</c>'s constructor.
		/// </summary>
		/// <param name = "parent">
		/// The window the <c>ConfirmDialog</c> instance is
		/// created from.
		/// </param>
		/// <param name = "question">
		/// The message which would be shown in the dialog.
		/// </param>
		private ConfirmDialog(Window parent, string question, params object[] args)
			: base(	parent,
					DialogFlags.DestroyWithParent,
					MessageType.Question, 
                    ButtonsType.YesNo,
                    question, args)
		{
			this.Title = "Pregunta";
			this.Modal = true;
			this.TransientFor = parent;
			this.Icon = parent.Icon;
		}	
		
		private new ResponseType Run()
		{
			return (ResponseType)(base.Run());		
		}
		
		/// <summary>
		/// Shows a Yes/No question dialog.
		/// </summary>
		/// <param name = "parent">
		/// The window the <c>ConfirmDialog</c> instance is
		/// created from.
		/// </param>
		/// <param name = "question">
		/// The message which would be shown in the dialog.
		/// </param>
		public static ResponseType Show(Window parent, string question, params object[] args)
		{
			ConfirmDialog cd = new ConfirmDialog(parent,question,args);
			
			ResponseType res = cd.Run();
			
			cd.Destroy();
			
			return res;
		}
	}
}
