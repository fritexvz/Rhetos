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
using System.Text;

namespace Rhetos.Dsl.DefaultConcepts
{
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("KeepSynchronized")]
    public class KeepSynchronizedInfo : IMacroConcept
    {
        [ConceptKey]
        public PersistedDataStructureInfo Persisted { get; set; }

        public IEnumerable<IConceptInfo> CreateNewConcepts(IEnumerable<IConceptInfo> existingConcepts)
        {
            return CreateNewConcepts(Persisted, existingConcepts, "");
        }

        public static IEnumerable<IConceptInfo> CreateNewConcepts(PersistedDataStructureInfo persisted, IEnumerable<IConceptInfo> existingConcepts, string filterSaveExpression)
        {
            var changesOnChangesItems = existingConcepts.OfType<ChangesOnChangedItemsInfo>()
                .Where(change => change.Computation == persisted.Source)
                .ToArray();

            var newConcepts = changesOnChangesItems.SelectMany(change =>
            {
                var readChanged = new ReadChangedItemsOnSaveInfo { DataStructure = change.DependsOn };
                var updateOnChange = new KeepSynchronizedOnChangedItemsInfo { Persisted = persisted, UpdateOnChange = change, ReadChanged = readChanged, FilterSaveExpression = filterSaveExpression };
                return new IConceptInfo[] { readChanged, updateOnChange };
            }).ToList();

            // If the computed data source is an extension, but its value does not depend on changes in its base data structure,
            // it should still be computed every time the base data structure data is inserted.

            DataStructureInfo dataSourceExtensionBase = existingConcepts.OfType<DataStructureExtendsInfo>()
                .Where(ex => ex.Extension == persisted.Source)
                .Select(ex => ex.Base).SingleOrDefault();
            
            if (dataSourceExtensionBase != null
                && dataSourceExtensionBase is IWritableOrmDataStructure
                && !changesOnChangesItems.Any(c => c.DependsOn == dataSourceExtensionBase)
                && !existingConcepts.OfType<ChangesOnBaseItemInfo>().Where(c => c.Computation == persisted.Source).Any())
                newConcepts.Add(new ComputeForNewBaseItemsWithFilterInfo { Persisted = persisted, FilterSaveExpression = filterSaveExpression });

            return newConcepts;
        }
    }
}