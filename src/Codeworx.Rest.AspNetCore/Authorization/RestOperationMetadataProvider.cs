﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

#if NETSTANDARD2_0

using Microsoft.AspNetCore.Mvc.Internal;

#endif

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class RestOperationMetadataProvider : IAttributeMetadataProvider
    {
        public void ProcessAction(IEnumerable<object> attributes, ActionModel action, MetadataProviderContext context)
        {
            var operation = attributes.OfType<RestOperationAttribute>().FirstOrDefault();

            if (operation == null)
            {
                context.ActionsToRemove.Add(action);
            }
            else
            {
                TransformAction(operation, action, context);
            }
        }

        public void ProcessController(IEnumerable<object> attributes, ControllerModel controller, MetadataProviderContext context)
        {
            // do nothing.
        }

        public void ProcessParameter(IEnumerable<object> attributes, ParameterModel parameter, MetadataProviderContext context)
        {
            // do nothing.
        }

        protected void TransformAction(RestOperationAttribute source, ActionModel model, MetadataProviderContext context)
        {
            var targetAttribute = GetTargetAttribute(source.HttpMethod(), source.Template);

            model.Selectors.Clear();
            model.Selectors.Add(CreateSelectorModel(targetAttribute, model.ActionMethod));
        }

        private static SelectorModel CreateSelectorModel(IRouteTemplateProvider route, MethodInfo method)
        {
            var selectorModel = new SelectorModel();
            if (route != null)
            {
                selectorModel.AttributeRouteModel = new AttributeRouteModel(route);
            }

            foreach (var item in method.GetCustomAttributes().OfType<IActionConstraintMetadata>())
            {
                selectorModel.ActionConstraints.Add(item);
                selectorModel.EndpointMetadata.Add(item);
            }

            selectorModel.EndpointMetadata.Add(route);

            // Simple case, all HTTP method attributes apply
            var httpMethods = new[] { route }
                .OfType<IActionHttpMethodProvider>()
                .SelectMany(a => a.HttpMethods)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (httpMethods.Length > 0)
            {
                selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(httpMethods));
                selectorModel.EndpointMetadata.Add(new HttpMethodMetadata(httpMethods));
            }

            return selectorModel;
        }

        private IRouteTemplateProvider GetTargetAttribute(string method, string template)
        {
            switch (method)
            {
                case "GET":
                    return template == null ? new HttpGetAttribute() : new HttpGetAttribute(template);

                case "POST":
                    return template == null ? new HttpPostAttribute() : new HttpPostAttribute(template);

                case "PUT":
                    return template == null ? new HttpPutAttribute() : new HttpPutAttribute(template);

                case "DELETE":
                    return template == null ? new HttpDeleteAttribute() : new HttpDeleteAttribute(template);

                case "HEAD":
                    return template == null ? new HttpHeadAttribute() : new HttpHeadAttribute(template);

                case "OPTIONS":
                    return template == null ? new HttpOptionsAttribute() : new HttpOptionsAttribute(template);

                case "PATCH":
                    return template == null ? new HttpPatchAttribute() : new HttpPatchAttribute(template);

                default:
                    throw new NotSupportedException($"Http method {method} is not supported by the rest contract package!");
            }
        }
    }
}
