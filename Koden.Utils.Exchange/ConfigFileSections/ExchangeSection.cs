#region License
// Copyright (c) 2014 Tim Fischer
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the 'Software'), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Linq;
using System.Configuration;

namespace Koden.Utils.Exchange.ConfigFileSections
{

    /// <summary>
    /// This class adds a custom section to the App.Config/Web.Config files labeled 
    /// </summary>
    /// <example>
    /// This is an example of an App.Config file:
    /// <code lang="xml" xml:space="preserve"> 
    /// <configuration>
    ///   <configSections>
    ///     <sectionGroup name="KodenGroup">
    ///       <section name="Exchange" type="Koden.Utils.ConfigFileSections.ExchangeSection,Koden.Utils.ExchangeClient" allowLocation="true" allowDefinition="Everywhere" />
    ///     </sectionGroup>
    ///   </configSections>
    ///     <startup> 
    ///         <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    ///     </startup>
    ///   <KodenGroup>
    ///        <Exchange>
    ///          <Mailboxes>
    ///            <Mailbox  name="OracleETL@fmgllc.com" userid="svc-appdev-ssis" password="password" subjectToMonitor="Subjec1" AttachmentExtpected="true"  />
    ///            <Mailbox  name="SomeOtherETL@fmgllc.com" userid="svc-appdev-ssis" password="password" subjectToMonitor="Subjec1" AttachmentExtpected="true" />
    ///          </Mailboxes>
    ///        </Exchange>
    ///    </KodenGroup>
    /// </configuration>     
    /// </code>
    ///</example>
    public class ExchangeSection : ConfigurationSection
    {
        [ConfigurationProperty("Mailboxes")]
        internal MailboxAppearanceCollection MailboxElement
        {
            get { return ((MailboxAppearanceCollection)(base["Mailboxes"])); }
            set { base["Mailboxes"] = value; }
        }
    }

}


