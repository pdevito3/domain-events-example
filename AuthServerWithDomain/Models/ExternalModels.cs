//This project uses Duende Software and parts of their templates (no affiliation with Wrapt or Craftsman).
//Please see the Duende Licensing information at https://duendesoftware.com/
// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


namespace AuthServerWithDomain.Models;  

public class ExternalProvider
{
    public string DisplayName { get; set; }
    public string AuthenticationScheme { get; set; }
}