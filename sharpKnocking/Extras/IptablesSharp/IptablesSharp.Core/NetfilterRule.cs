// NetfilterRule.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using IptablesSharp.Core;
using IptablesSharp.Core.Options;
using IptablesSharp.Core.Commands;
using IptablesSharp.Core.Extensions.ExtendedMatch;
using IptablesSharp.Core.Extensions.ExtendedTarget;

using Developer.Common.Options;

namespace IptablesSharp.Core
{
	
	/// <summary>
	/// Base class for rules. It store an array of generic parameters with
	/// all the parameters that the rule contains. The rule also has the
	/// command parameter included.
	/// </summary>
	/// <remarks>
	/// To use match extension options in the rule you have to add an option
	/// to load explicitly the extension and then you can access the extension
	/// handler from the list and add the options to the extension handler object.
	/// </remarks>
	public class NetfilterRule: IComparable <NetfilterRule>
	{
	    private NetfilterChain parentChain;
	    
	    /// <summary>
	    /// Table where this rule is defined.
	    /// </summary>
	    /// <remarks>
	    /// Adding an option with the -t parameter to change the target table
	    /// doesn't change this value but it will cause a runtime exception
	    /// when trying to convert this rule as text.
	    /// </remarks>
	    public NetfilterChain ParentChain
	    {
	        get {return this.parentChain;}
	        set {this.parentChain = value;}
	    }
	    
	    private List<MatchExtensionHandler> loadedExtensions;
		private ReadOnlyMatchExtensionListAdapter adapter;
	    
	    /// <summary>
	    /// Gets the list of loaded extensions in the rule.
	    /// </summary>
	    /// <remarks>
	    /// Adding a extension is the same as adding the -m option to the
	    /// command line. But there is one main difference, here the extended
	    /// options must be added to the extension directly, not to the list
	    /// of normal options (see Options property).
	    /// </remarks>
	    public ReadOnlyMatchExtensionListAdapter LoadedExtensions
	    {
	        get
	        {
	            //We return a reference to the adapter that will do read-only
	            //access to the list
	           return adapter;
	        }
	    }
	    
	    private RuleOptionList options;
	    
	    /// <summary>
	    /// Gets the array of options for this rule.
	    /// </summary>
	    public RuleOptionList Options
	    {
	        get
	        {
	            return this.options;
	        }
	    }

	    private JumpOption jumpOption;
	    
	    /// <summary>
	    /// Default constructor.
	    /// </summary>
		public NetfilterRule()
		{
		    this.options = new RuleOptionList(this);
		    this.loadedExtensions = new List<MatchExtensionHandler>();
			this.adapter = new ReadOnlyMatchExtensionListAdapter(this.loadedExtensions);
		    this.options.ItemAdded += new GenericOptionListEventHandler(this.ItemAddedHandler);
		    this.options.ItemRemoved += new GenericOptionListEventHandler(this.ItemRemovedHandler);
		    this.options.ItemsCleared += new EventHandler(this.ItemsClearedHandler);
		}
		
		/// <summary>
		/// Handles the event produced when new items are added to a chain.
		/// </summary>
		/// <param name="obj">Object that produced the event handled.</param>
		/// <param name="args">Arguments of the event</param>
		/// <remarks>
		/// When a item is added it is needed to check the type, if the item is an extension
		/// we will need to load the extension to know how to handle it and check if the values
		/// are properly set.
		/// </remarks>
		private void ItemAddedHandler(object obj, ListChangedEventArgs<GenericOption> args)
		{
			//Console.WriteLine("ItemAdded: "+args.Item.GetType().Name+", "+args.Item.ParentRule);
			//We have to load the concrete handler for each extension
		    if(args.Item.HasImplicitExtension)
		    {
				if(args.Item.ExtensionType==null)
					throw new InvalidOperationException("The added item declares to have an implicit extension but no type was found");
				//If it is loaded exit, if not load it
    		    if(this.IsExtensionHandlerLoaded(args.Item.ExtensionType))
    		        return;
    		    
                this.LoadExtensionHandler(args.Item.ExtensionType);
		    }
		    else if(args.Item is MatchExtensionOption)
		    {
				if(args.Item.ExtensionType==null)
					throw new InvalidOperationException("The added match extension doesn't have a type!");
		        //If it is an extension we load the extension handler
		        this.LoadExtensionHandler(args.Item.ExtensionType);
		    }
		    else if(args.Item is JumpOption)
		    {
		        //we keep a reference to this option to check names
		        this.jumpOption = (JumpOption)args.Item;
		    }
		}
		
		private void LoadExtensionHandler(Type mExtension)
		{			
            //The factory known how to load the extension
            MatchExtensionHandler handler = MatchExtensionFactory.GetExtension(mExtension);
            this.loadedExtensions.Add(handler);
		}
		
		private void UnloadExtensionHandler(Type extType)
		{
			int pos=0;
			//we must find it before removing
			while(pos<this.loadedExtensions.Count)
			{
				if(this.loadedExtensions[pos].GetType()==extType)
				{
					//This removes everything as the options belongs to the extension itself
					this.loadedExtensions.RemoveAt(pos);
					return;
				}
			}
		}
		
		private bool IsExtensionHandlerLoaded(Type extHandlerType)
		{
            for(int i=0;i<this.loadedExtensions.Count;i++)
            {
                if(this.loadedExtensions[i].GetType() == extHandlerType)
                    return true;
            }
            
            return false;
		}
		
		private void ItemRemovedHandler(object obj, ListChangedEventArgs<GenericOption> args)
		{
		    //If an extension option is removed we must remove that extension with
		    //all the options.
		    
		    if(!args.Item.HasImplicitExtension)
		        return;
		    else if(args.Item is JumpOption)
		    {
		        this.jumpOption = null;    
		    }
		    
		    this.UnloadExtensionHandler(args.Item.ExtensionType);
		}
		
		private void ItemsClearedHandler(object obj, EventArgs args)
		{
		    //When all the items are removed we must remove all the extensions too
		    this.loadedExtensions.Clear();
		}
		
		/// <summary>
		/// Checks if the current target extension can handle the parameter and
		/// returns it
		/// </summary>
		/// <remarks>
		/// The extension targets are specified throught the jump option
		/// <c>Options.JumpOption</c>
		/// </remarks>
		/// <returns>
		/// True if it can handle the parameter or false if not
		/// </returns>
		public bool TryGetTargetExtensionHandler(string paramName, out TargetExtensionHandler handler)
		{
			handler = null;
			
			//We only have to look at the target and ask him
		    if(this.jumpOption!=null && this.jumpOption.HasOptionNamed(paramName)) {
				handler = this.jumpOption.Extension;
				return true;
			}
		    
		    return false;
		}
		
		/// <summary>
		/// Tries to get the first match extensin handler object that can handle a specified parameter
		/// from those loaded into the rule.
		/// </summary>
		/// <param name="paramName">
		/// Name of the parameter
		/// </param>
		/// <param name="handler">Handler instance in the rule that can handle the parameter</param>
		/// <returns>
		/// True if one handler was found  or false if not
		/// </returns>
		public bool TryGetMatchExtensionHandler(string paramName, out MatchExtensionHandler handler)
		{
			handler = null;
			
			//Look at every extension for one that will support the parameter
		    for(int i=0;i<this.loadedExtensions.Count;i++) {
		        handler = this.loadedExtensions[i];
		        if(handler.IsSupportedParam(paramName))
                    return true;
		    }
			
		    handler = null;
			return false;
		}
		
		/// <summary>
		/// Returns an instance of the handler for the parameter.
		/// </summary>
		/// <remarks>
		/// If it is a parameter supported by the current loaded target extension
		/// the extension is returned but if the extension doesn't support the
		/// parameter or there is no extension target loaded it returns null.
		/// </remarks>
		public TargetExtensionHandler FindTargetExtensionHandler(string paramName)
		{
		    if(this.jumpOption!=null &&
		             this.jumpOption.HasOptionNamed(paramName))
		    {
		        return this.jumpOption.Extension;
		    }
		    
		    return null;
		}
		
		/// <summary>
		/// Iterates the list of loaded handlers searching for one that supports
		/// the parameter.
		/// </summary>
		/// <returns>
		/// The instance of the loaded handler that supports the parameter or
		/// null if it doesn't exist
		/// </returns>
		public MatchExtensionHandler FindMatchExtensionHandlerFor(string paramName)
		{
		    MatchExtensionHandler handler;
		    
		    for(int i=0;i<this.loadedExtensions.Count;i++)
		    {
		        handler = this.loadedExtensions[i];

		        if(handler.IsSupportedParam(paramName))
                    return handler;
		    }
		    
		    return null;
		}
		
		/// <summary>
		/// Tries to find a match extension handler for an option
		/// </summary>
		/// <param name="meo">
		/// A <see cref="MatchExtensionOption"/>
		/// </param>
		/// <returns>
		/// A <see cref="MatchExtensionHandler"/> that owns the option
		/// </returns>
		public MatchExtensionHandler FindMatchExtensionHandler(MatchExtensionOption meo)
		{
			for (int i=0;i<this.loadedExtensions.Count;i++) {
				if(loadedExtensions[i].GetType() == meo.ExtensionType) {
					return loadedExtensions[i];
				}
			}
			
			return null;
		}
		
		
		public void AppendContentsTo(StringBuilder sb, StringFormatOptions options)
		{
			//Console.WriteLine("** Converting rule to string ** ");
		    for(int i=0;i<this.options.Count;i++)
		    {
				if(this.options[i]==this.jumpOption)
					continue;
				else if(this.options[i] is MatchExtensionOption)
				{
					//First we print the option and then the parameters
					sb.Append(this.options[i].ToString()+" ");
					MatchExtensionHandler handler = this.FindMatchExtensionHandler((MatchExtensionOption)this.options[i]);
					if(handler!=null)
					{
						handler.AppendContentsTo(sb);
						sb.Append(" ");
					}
					//Console.WriteLine("MatchExtension: "+this.options[i]+" "+handler);
				}
				else
				{
					//Console.WriteLine("Option: "+this.options[i]);
					sb.Append(this.options[i].ToString()+" ");
				}
		    }
			
			//Console.WriteLine("Target extension: "+jumpOption);
			if(jumpOption!=null)
				sb.Append(this.jumpOption.ToString());
		}
		
		public string GetContentsAsString(StringFormatOptions options)
		{
			StringBuilder sb = new StringBuilder();
			this.AppendContentsTo(sb, options);
			return sb.ToString();
		}
		
		//---------------------------------------------------
		// IComparable<T> implementation and overrides of ToString, Equals and
		// GetHashCode.
				
		public override string ToString ()
		{
			return this.GetContentsAsString(StringFormatOptions.Default);
		}
		
		public override bool Equals (object o)
		{
			if(!(o is NetfilterRule) || o == null)
				return false;
			
			string strThis = this.ToString ();
			return strThis.Equals(o.ToString(), StringComparison.InvariantCultureIgnoreCase);
		}

		public override int GetHashCode ()
		{
			return this.ToString ().GetHashCode ();
		}
		
		public int CompareTo (NetfilterRule rule)
		{
			if(rule==null)
				throw new ArgumentNullException ("rule");
				
			string strThis = this.ToString ();
			return strThis.CompareTo (rule.ToString());
		}

	}
}
