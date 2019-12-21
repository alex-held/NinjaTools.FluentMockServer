using JetBrains.Annotations;
using TestStack.BDDfy;

namespace NinjaTools.FluentMockServer.API.Tests.Downstream
{
    // /// <inheritdoc />
    // public class EmailExampleTable : ExampleTable
    // {
    //     [NotNull] 
    //     public static EmailExampleTable Examples => new EmailExampleTable();
    //     
    //     public EmailExampleTable() : base("From", "To", "Subject", "Content")
    //     {
    //         var mail = RandomEmailFactory.GenerateOneRandomEmail();
    //
    //         Add(new Example(
    //             new ExampleValue("From", mail.From, () => 1),
    //             new ExampleValue("To", mail.To, () => 1),
    //             new ExampleValue("Subject", mail.Subject, () => 1),
    //             new ExampleValue("Content", mail.Content.ToString(), () => 1)));
    //
    //     }
    // }
}
