using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
        Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>,
        IHandler<CreateCreditCardSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(
            IStudentRepository repository,
            IEmailService emailService
        )
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            if (_repository.EmailExists(command.Document))
                AddNotification("Email", "Este E-mail já está em uso");

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var payerDocument = new Document(command.PayerDocument, command.PayerDocumentType);
            var payerEmail = new Email(command.PayerEmail);
            var address = new Address(
                command.Street,
                command.Number,
                command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode
            );

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                payerDocument,
                address,
                payerEmail,
                command.BarCode,
                command.BoletoNumber
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(
                name,
                document,
                email,
                payerDocument,
                payerEmail,
                address,
                student,
                subscription,
                payment
            );

            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            _repository.CreateSubscription(student);

            _emailService.Send(
                student.Name.ToString(),
                student.Email.Address,
                "bem vindo ao balta.io",
                "Sua assinatura foi criado"
            );

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            if (_repository.EmailExists(command.Document))
                AddNotification("Email", "Este E-mail já está em uso");

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var payerDocument = new Document(command.PayerDocument, command.PayerDocumentType);
            var payerEmail = new Email(command.PayerEmail);
            var address = new Address(
                command.Street,
                command.Number,
                command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode
            );

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                payerDocument,
                address,
                payerEmail,
                command.TransactionCode
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(
                name,
                document,
                email,
                payerDocument,
                payerEmail,
                address,
                student,
                subscription,
                payment
            );

            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            _repository.CreateSubscription(student);

            _emailService.Send(
                student.Name.ToString(),
                student.Email.Address,
                "bem vindo ao balta.io",
                "Sua assinatura foi criado"
            );

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreateCreditCardSubscriptionCommand command)
        {
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            if (_repository.EmailExists(command.Document))
                AddNotification("Email", "Este E-mail já está em uso");

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var payerDocument = new Document(command.PayerDocument, command.PayerDocumentType);
            var payerEmail = new Email(command.PayerEmail);
            var address = new Address(
                command.Street,
                command.Number,
                command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode
            );

            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new CreditCardPayment(
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                payerDocument,
                address,
                payerEmail,
                command.CardHolderName,
                command.CardNumber,
                command.LastTransactionNumber
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(
                name,
                document,
                email,
                payerDocument,
                payerEmail,
                address,
                student,
                subscription,
                payment
            );

            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            _repository.CreateSubscription(student);

            _emailService.Send(
                student.Name.ToString(),
                student.Email.Address,
                "bem vindo ao balta.io",
                "Sua assinatura foi criado"
            );

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}