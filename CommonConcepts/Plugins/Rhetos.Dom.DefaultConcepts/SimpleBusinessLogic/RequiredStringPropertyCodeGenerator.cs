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
using Rhetos.Dsl.DefaultConcepts;
using System.Globalization;
using System.ComponentModel.Composition;
using Rhetos.Extensibility;
using Rhetos.Dsl;
using Rhetos.Compiler;

namespace Rhetos.Dom.DefaultConcepts
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(RequiredPropertyInfo))]
    public class RequiredStringPropertyCodeGenerator : IConceptCodeGenerator
    {
        private string CheckDataSnippet(RequiredPropertyInfo info)
        {
            return string.Format(
@"            if (inserted.Any(item => string.IsNullOrWhiteSpace(item.{2})) || updated.Any(item => string.IsNullOrWhiteSpace(item.{2})))
                throw new Rhetos.UserException(""It is not allowed to enter {0}.{1} because the required property {2} is not set."");

",
                info.Property.DataStructure.Module.Name,
                info.Property.DataStructure.Name,
                info.Property.Name);
        }

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (RequiredPropertyInfo)conceptInfo;
            if (RequiredPropertyCodeGenerator.IsSupported(info.Property)
                && (info.Property is ShortStringPropertyInfo || info.Property is LongStringPropertyInfo))
            {
                codeBuilder.InsertCode(CheckDataSnippet(info), WritableOrmDataStructureCodeGenerator.NewDataLoadedTag, info.Property.DataStructure);
                codeBuilder.AddReferencesFromDependency(typeof(UserException));
            }
        }
    }
}