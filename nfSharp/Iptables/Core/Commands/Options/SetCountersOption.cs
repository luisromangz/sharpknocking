// SetCountersOption.cs
//
//  Copyright (C) 2006 SharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
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
using System;

using NFSharp.Iptables;

namespace NFSharp.Iptables.Core.Commands.Options {


    public class SetCountersOption: GenericOption {

        public SetCountersOption()
        :base(RuleOptions.SetCounters) {
            throw new NotImplementedException("A lazy programmer didn't implemented "+
                                              "this class properly.");
        }


        public override bool TryReadValues (string strVal, out string errStr) {
            throw new NotImplementedException("A lazy programmer didn't implemented "+
                                              "this class properly.");
        }

        protected override string GetValueAsString() {
            throw new NotImplementedException("O_o");
        }

    }
}
