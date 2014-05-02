// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
// WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF
// TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR
// NON-INFRINGEMENT.
// See the Apache 2 License for the specific language governing
// permissions and limitations under the License.

using Microsoft.Data.Entity;
using Moq;
using Xunit;

namespace Microsoft.Data.SqlServer.Tests
{
    public class SqlServerDataStoreSourceTest
    {
        [Fact]
        public void Returns_appropriate_name()
        {
            Assert.Equal(typeof(SqlServerDataStore).Name, new SqlServerDataStoreSource().Name);
        }

        [Fact]
        public void Is_configured_when_configuration_contains_associated_extension()
        {
            var configuration = new EntityConfigurationBuilder()
                .AddBuildAction(c => c.AddOrUpdateExtension<SqlServerConfigurationExtension>(e => { }))
                .BuildConfiguration();

            var configurationMock = new Mock<ContextConfiguration>();
            configurationMock.Setup(m => m.EntityConfiguration).Returns(configuration);

            Assert.True(new SqlServerDataStoreSource().IsConfigured(configurationMock.Object));
        }

        [Fact]
        public void Is_not_configured_when_configuration_does_not_contain_associated_extension()
        {
            var configuration = new EntityConfigurationBuilder().BuildConfiguration();

            var configurationMock = new Mock<ContextConfiguration>();
            configurationMock.Setup(m => m.EntityConfiguration).Returns(configuration);

            Assert.False(new SqlServerDataStoreSource().IsConfigured(configurationMock.Object));
        }

        [Fact]
        public void Is_available_when_configured()
        {
            var configuration = new EntityConfigurationBuilder()
                .AddBuildAction(c => c.AddOrUpdateExtension<SqlServerConfigurationExtension>(e => { }))
                .BuildConfiguration();

            var configurationMock = new Mock<ContextConfiguration>();
            configurationMock.Setup(m => m.EntityConfiguration).Returns(configuration);

            Assert.True(new SqlServerDataStoreSource().IsAvailable(configurationMock.Object));
        }

        [Fact]
        public void Is_not_available_when_not_configured()
        {
            var configuration = new EntityConfigurationBuilder().BuildConfiguration();

            var configurationMock = new Mock<ContextConfiguration>();
            configurationMock.Setup(m => m.EntityConfiguration).Returns(configuration);

            Assert.False(new SqlServerDataStoreSource().IsAvailable(configurationMock.Object));
        }
    }
}