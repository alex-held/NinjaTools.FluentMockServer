# Roadmap

Currently there is no stable release shedule planned.
Here are the topics I'm looking forward to integrate in the sort term future.

## Postman Documentation

To reduce overhead when starting to work with the Mock-Server, there will be be a hosted Postman Collection, which will also serve as an API documentation.

## Response Configuration

Right now the Mock-Server only supports configuring static `Responss`. While this is enough for most cases,
in some cases you may want to set up the Mock-Server in a more dynamic fashion.

### Code Execution

The Mock-Server should be able to execute arbituary code to generate a `Response` when a `Request` has been matched.

Following languages are currently planned to supported:

- [ ] `C#`
- [ ] `CSX` _([csharp script](http://scriptcs.net/))_
- [ ] `Python`
- [ ] `FSharp`
- [ ] `VB.Net`
- [ ] `JavaScript`

Please create an Issue if you have any suggestions.

### Forwarding

Proxy capabilities will come in the future to forward request conditionally. The main use case will be intercepting and modifiying the `Request` and or `Responses` of an existing ressource.

## SSL

To be as close to real world as possible there is no way around Https communication. A server certificate will be added to the Mock-Server image and will offer you an easy way to setup up a secure connection.

## AdminUI

Sometimes you may want to operate the Mock-Server via a WebUI. We're looking forward to craft a simple frontend application.

## Authentication

Provide various authentication methods in the `RequestMatcher`.

- [ ] Basic Auth
- [ ] API Key
- [ ] Bearer Token
- [ ] Digest Auth
- [ ] OAuth 1.0
- [ ] OAuth 2.0
- [ ] Hawk Authentication
- [ ] AWS Signature
- [ ] Akamai EdgeGrid
- [ ] KeyCloak

## OpenAPI Auto Config

It would be extremly helpful to just provide an OpenApi2 / OpenApi3 config file and the MockServer will provide you with deterministic data.

## CI / CD Gates

To even more simplify your testing we want to provide a way to integrate an integration test runner for various CI pipelines.
