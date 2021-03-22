# DeepSleep

[![Build Status](https://dev.azure.com/mtnvencenzo/DeepSleep/_apis/build/status/mtnvencenzo.DeepSleep?branchName=master&jobName=Job)](https://dev.azure.com/mtnvencenzo/DeepSleep/_build/latest?definitionId=17&branchName=master) [![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/mtnvencenzo/DeepSleep/blob/master/LICENSE) [![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://github.com/mtnvencenzo/DeepSleep/graphs/commit-activity)

View the [documentation site](https://deepsleep-doc.azurewebsites.net) for more detailed information.

## DeepSleep Nuget Packages

| Package Name | Target Framework(s) | Version |
| -------------- | :-------: | :------- | 
| [DeepSleep](https://www.nuget.org/packages/DeepSleep) | netstandard2.0 | ![DeepSleep Nuget Package](https://buildstats.info/nuget/deepsleep) |
| [DeepSleep.Web](https://www.nuget.org/packages/DeepSleep.Web) | net5.0<br/>netcoreapp3.1<br/>netcoreapp2.1 | ![DeepSleep.Web Nuget Package](https://buildstats.info/nuget/deepsleep.web) |
| [DeepSleep.OpenApi](https://www.nuget.org/packages/DeepSleep.OpenApi)  | netstandard2.0 | ![DeepSleep.OpenApi Nuget Package](https://buildstats.info/nuget/deepsleep.openapi) |


## List of Features

* **Route Discovery** - The process of the DeepSleep runtime identifing and enroling your API routes into registered endpoints.  Support for automated creation of HEAD routes for registered GET endpoints.
  * [Attribute Routing](https://deepsleep-doc.azurewebsites.net/route-discovery/attribute-routing/attribute-routing) - The simplist and easiest to use form of discovery
    * Full support for routing and request configuration via method attributes.
  * [Static Routing](https://deepsleep-doc.azurewebsites.net/route-discovery/static-routing/static-routing) - Allows for pulling routing meta-data from external sources or defined methods handling route information and configuration
* **Request Pipeline** - The core middleware tasked with processing API requests.
  * [Authentication](https://deepsleep-doc.azurewebsites.net/request-pipeline/authentication/overview) - Extensible middleware to enforce request authentication.
    * Create and plug in authentication components into the request pipeline eith globally or at an endpoint level.
    * Support for multiple authentication methods defined for endpoints.
    * Opt-in/Opt-out of authentication either globally or at an individual endpoint.
  * [Authorization](https://deepsleep-doc.azurewebsites.net/request-pipeline/authorization/overview) - Extensible middleware to enforce request authorization.
    * Create and plug in authorization components into the request pipeline eith globally or at an endpoint level.
    * Support for multiple authorization methods defined for endpoints.
    * Opt-in/Opt-out of authorization either globally or at an individual endpoint.
  * [Cross Origin Resource Sharing (CORS)](https://deepsleep-doc.azurewebsites.net/request-pipeline/cors/overview) - Middleware to support CORS pre-flight and standard requests.
    * Customize allowable origins, exposable headers and allowable request headers.
    * Supports pre-flight and standard CORS requests.
  * [Content Negotiation](https://deepsleep-doc.azurewebsites.net/content-negotiation/overview) - Customizable support for media serializers to process inbound request and out going response bodies.
    * Built-in JSON media serializer using the `System.Text.Json` serializer.  Create your own, or modifiy the existing serializer options.
    * Built-in XML media serializer.
    * Built-in form url encoded media serializer to accept `application/x-www-form-urlencoded` content-types.
    * Built in multipart/form-data medai serializer for request bodies.
  * [Model Binding](https://deepsleep-doc.azurewebsites.net/request-pipeline/model-binding/overview) - Complex request uri and body object model support. Supports custom poco object binding for Uri and Body as well as simple binding for primitive/simple type method paramter binding.
  * [4xx-5xx Response Handling](https://deepsleep-doc.azurewebsites.net/request-pipeline/error-handling/error-responses) - Customizable support for standard 4xx and 5xx error responses.  Global handler for standardized responses orb customize for more granular error responses.
  * [Routing](https://deepsleep-doc.azurewebsites.net/request-pipeline/routing/overview) - Full featured request routing framework.  
  * [Validation](https://deepsleep-doc.azurewebsites.net/request-pipeline/validation/overview) - Common validation middleware for validating bound pcoc's, simple parameters and giving the ability to validate the full request with access to all request inputs.
  * [Response Helpers](https://deepsleep-doc.azurewebsites.net/request-pipeline/response-helpers/overview) - Helper methods/objects that allow for better control over the full HTTP response.
  * [Localization](https://deepsleep-doc.azurewebsites.net/request-pipeline/localization/overview) - Middleware support for managing the request thread's culture and ui-culture based on the incoming request.
  * [Request Logging](https://deepsleep-doc.azurewebsites.net/request-pipeline/request-logging/global-handler) - Global handler for logging the full HTTP request.
  * [Exception Handling](https://deepsleep-doc.azurewebsites.net/request-pipeline/exception-handling/global-handler) - Global handler for catching all unhandled exceptions.

## Samples

Sample projects can be found in the $/samples directory

#### $/Samples.Simple.Api
This is a basic sample that illustrates a basic bootraping using all the default configurations.  Includes a simple *Hello World* endpoint.

#### $/Samples.Mixed.Api
This is a basic sample that illustrates a basic bootraping using all the default configurations but also shows how to include/exclude routes to use with mixing DeepSleep apis along side Razor and/or Static files or SPA.  Includes a simple *Hello World* endpoint.

