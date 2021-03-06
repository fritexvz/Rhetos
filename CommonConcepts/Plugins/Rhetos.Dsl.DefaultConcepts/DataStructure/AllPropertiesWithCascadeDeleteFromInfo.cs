﻿/*
    Copyright (C) 2013 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rhetos.Dsl.DefaultConcepts
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("AllPropertiesWithCascadeDeleteFrom")]
    public class AllPropertiesWithCascadeDeleteFromInfo : AllPropertiesFromInfo, IMacroConcept
    {
        new public IEnumerable<IConceptInfo> CreateNewConcepts(IEnumerable<IConceptInfo> existingConcepts)
        {
            var newConcepts = new List<IConceptInfo>();
            
            newConcepts.AddRange(base.CreateNewConcepts(existingConcepts));

            newConcepts.AddRange(existingConcepts.OfType<ReferenceCascadeDeleteInfo>().Where(ci => ci.Reference.DataStructure == Source)
                .Select(ci => new ReferenceCascadeDeleteInfo
                {
                    Reference = new ReferencePropertyInfo
                        {
                            DataStructure = Destination,
                            Name = ci.Reference.Name,
                            Referenced = ci.Reference.Referenced
                        }
                }));

            return newConcepts;
        }
    }
}
