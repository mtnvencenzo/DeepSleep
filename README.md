# DeepSleep

[![Build Status](https://dev.azure.com/mtnvencenzo/DeepSleep/_apis/build/status/mtnvencenzo.DeepSleep?branchName=master&jobName=Job)](https://dev.azure.com/mtnvencenzo/DeepSleep/_build/latest?definitionId=17&branchName=master) [![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/mtnvencenzo/DeepSleep/blob/master/LICENSE) [![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://github.com/mtnvencenzo/DeepSleep/graphs/commit-activity)

View the [documentation site](https://deepsleep-doc.azurewebsites.net) for more detailed information.

## DeepSleep Nuget Packages

| Package Name | Target(s) | .NET Standard |
| -------------- | :-------: | :-------: | 
| [DeepSleep](https://www.nuget.org/packages/DeepSleep) | 2.0 | 2.0 |
| [DeepSleep.Web](https://www.nuget.org/packages/DeepSleep.Web) | 2.0 | 2.0 |
| [DeepSleep.OpenApi](https://www.nuget.org/packages/DeepSleep.OpenApi)  | 4.5 | 2.0 |

## List of Features

* **Route Discovery** - The process of the DeepSleep runtime identifing and enroling your API routes into registered endpoints.
  * [Attribute Routing](https://deepsleep-doc.azurewebsites.net/route-discovery/attribute-routing/attribute-routing) - The simplist and easiest to use form of discovery
  * [Static Routing](https://deepsleep-doc.azurewebsites.net/route-discovery/static-routing/static-routing) - Allows for pulling routing meta-data from external sources or defined methods handling route information and configuration
* **Request Pipeline** - The core middleware tasked with processing API requests.
  * [Authentication](https://deepsleep-doc.azurewebsites.net/request-pipeline/authentication/overview) - Extensible middleware to enforce request authentication.
  * [Authorization](https://deepsleep-doc.azurewebsites.net/request-pipeline/authorization/overview) - Extensible middleware to enforce request authorization.
  * [Cross Origin Resource Sharing (CORS)](https://deepsleep-doc.azurewebsites.net/request-pipeline/cors/overview) - Middleware to support CORS pre-flight and standard requests.
  * [Content Negotiation](https://deepsleep-doc.azurewebsites.net/content-negotiation/overview) - Customizable support for media serializers to process inbound request and out going response bodies.
  * [Model Binding](https://deepsleep-doc.azurewebsites.net/request-pipeline/model-binding/overview) - Complex request uri and body object model support. Supports custom poco object binding for Uri and Body as well as simple binding for primitive/simple type method paramter binding.
  * [4xx-5xx Response Handling](https://deepsleep-doc.azurewebsites.net/request-pipeline/error-handling/error-responses) - Customizable support for standard 4xx and 5xx error responses.  Global handler for standardized responses orb customize for more granular error responses.
  * [Routing](https://deepsleep-doc.azurewebsites.net/request-pipeline/routing/overview) - Full featured request routing framework.
  * [Validation](https://deepsleep-doc.azurewebsites.net/request-pipeline/validation/overview) - Common validation middleware for validating bound pcoc's, simple parameters and giving the ability to validate the full request with access to all request inputs.
