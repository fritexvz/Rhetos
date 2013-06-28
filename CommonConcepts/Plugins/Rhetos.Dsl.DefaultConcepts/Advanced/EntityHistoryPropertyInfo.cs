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
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Rhetos.Utilities;

namespace Rhetos.Dsl.DefaultConcepts
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("History")]
    public class EntityHistoryPropertyInfo : IMacroConcept
    {
        [ConceptKey]
        public PropertyInfo Property { get; set; }

        public IEnumerable<IConceptInfo> CreateNewConcepts(IEnumerable<IConceptInfo> existingConcepts)
        {
            var newConcepts = new List<IConceptInfo>();

            var entityHistory = existingConcepts.OfType<EntityHistoryInfo>().Where(eh => eh.Entity == Property.DataStructure).SingleOrDefault();
            if (entityHistory == null)
            {
                entityHistory = new EntityHistoryInfo { Entity = (EntityInfo)Property.DataStructure };
                newConcepts.Add(entityHistory);
            }

            var entityHistoryEx = existingConcepts.OfType<EntityHistoryExInfo>().Where(eh => eh.Entity == Property.DataStructure).SingleOrDefault();
            if (entityHistoryEx != null)
                newConcepts.Add(new EntityHistoryPropertyExInfo { Property = Property, EntityHistory = entityHistoryEx });

            return newConcepts;
        }
    }
}