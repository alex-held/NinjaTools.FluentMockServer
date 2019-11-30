using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.API.Services
{
    public class ExpectationService
    {
        private readonly List<Expectation> _expectations;
        
        public ExpectationService()
        {
            _expectations= new List<Expectation>();
            Seed(5);
        }


        public IEnumerable<Expectation> GetExpectations(Func<Expectation, bool> predicate = null)
        {
            return _expectations.Where(predicate ?? (e => true)).ToArray();
        }

        public int Prune()
        {
            var count = _expectations.Count;
            _expectations.Clear();
            return count;
        }

        public void Add(Expectation expectation) => _expectations.Add(expectation);
        
        public void Seed(int count)
        {
            if (count >= 1)
            {
                var expectation = new Expectation
                {
                    HttpRequest = new HttpRequest
                    {
                        Method = "GET",
                        Path = "/home"
                    },
                    HttpError = new HttpError
                    {
                        DropConnection = true,
                        Delay = new Delay
                        {
                            Value = 4,
                            TimeUnit = TimeUnit.Seconds
                        }
                    }
                };
                        
                _expectations.Add(expectation);
            }
            if (count >= 1)
            {
                var expectation = new Expectation
                {
                    Times = new Times
                    {
                        Unlimited = true
                    },
                    HttpResponse = new HttpResponse
                    {
                        StatusCode = 201
                    }
                };
                        
                _expectations.Add(expectation);
            }
            if (count >= 2)
            {
                var expectation = new Expectation
                {
                    Times = Times.Once,
                    HttpRequest = new HttpRequest
                    {
                        Method = "PUT"
                    },
                    HttpResponse = new HttpResponse
                    {
                        StatusCode = 500
                    }
                };
                        
                _expectations.Add(expectation);
            }
            if (count >= 3)
            {
                var expectation = new Expectation
                {
                    Times = Times.Once,
                    HttpRequest = new HttpRequest
                    {
                        Method = "POST",
                        Path = "/some/test",
                        Body = JToken.FromObject(new {type = "JSON", json = "json-body"})
                    },
                    HttpResponse = new HttpResponse
                    {
                        StatusCode =200
                    }
                };
                
                _expectations.Add(expectation);
            }
            if (count >= 4)
            {
                var expectation = new Expectation
                {
                    Times = Times.Once,
                    HttpRequest = new HttpRequest
                    {
                        Method = "POST",
                        Path = "/some/test"
                    },
                    HttpResponse = new HttpResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadGateway
                    }
                };
            
                _expectations.Add(expectation);
            }
        }
        
    }
}
