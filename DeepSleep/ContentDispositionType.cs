namespace DeepSleep
{

    /// <summary>In [RFC2183] there is a discussion of the "Content-Disposition" header
    /// field and the description of the initial values allowed in this
    /// header.  Additional values may be registered with the IANA
    /// following the procedures in section 9 of [RFC2183].  See http://www.iana.org/assignments/cont-disp/cont-disp.xhtml</summary>
    public enum ContentDispositionType
    {
        /// <summary>
        /// The none
        /// </summary>
        None                    = 0,

        /// <summary>
        /// Athenticated Identity Body - http://tools.ietf.org/html/rfc3893
        /// </summary>
        aib                     = 1,

        /// <summary>
        /// The body is a custom ring tone to alert the user
        /// </summary>
        alert                   = 2,

        /// <summary>
        /// User controlled display
        /// </summary>
        attachment              = 3,

        /// <summary>
        /// The body needs to be handled according to a reference to the body that is located in the same SIP message as the body.
        /// </summary>
        by_reference            = 4,

        /// <summary>
        /// The body describes an early communications session, for example, and [RFC2327] SDP body
        /// </summary>
        early_session           = 5,

        /// <summary>
        /// Process as form response
        /// </summary>
        form_data               = 6,

        /// <summary>
        /// The body is displayed as an icon to the user
        /// </summary>
        icon                    = 7,

        /// <summary>
        /// The body contains information associated with an Info Package
        /// </summary>
        info_package            = 8,

        /// <summary>
        /// Diplayed automatically
        /// </summary>
        inline                  = 9,

        /// <summary>
        /// The payload of the message carrying this Content-Disposition header field value is an Instant Message Disposition Notification as requested in the corresponding Instant Message.
        /// </summary>
        notification            = 10,

        /// <summary>
        /// The body includes a list of URIs to which URI-list services are to be applied.
        /// </summary>
        recipient_list          = 11,

        /// <summary>
        /// The body contains a list of URIs that indicates the recipients of the request
        /// </summary>
        recipient_list_history  = 12,

        /// <summary>
        /// The body should be displayed to the user
        /// </summary>
        render                  = 13,

        /// <summary>
        /// The body describes a communications session, for example, an RFC2327 SDP body
        /// </summary>
        session                 = 14,

        /// <summary>
        /// Tunneled content to be processed silently
        /// </summary>
        signal                  = 15
    }
}
