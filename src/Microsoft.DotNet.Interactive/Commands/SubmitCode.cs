﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.DotNet.Interactive.Parsing;

namespace Microsoft.DotNet.Interactive.Commands
{
    public class SubmitCode : KernelCommandBase
    {
        public SubmitCode(
            string code,
            string targetKernelName = null,
            SubmissionType submissionType = SubmissionType.Run) : base(targetKernelName)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            SubmissionType = submissionType;
        }

        internal SubmitCode(
            LanguageNode languageNode,
            SubmissionType submissionType = SubmissionType.Run,
            IKernelCommand parent = null) :
            base(languageNode.Language, parent)
        {
            Code = languageNode.Text;
            SubmissionType = submissionType;
            SuppressSplit = true;
        }

        public string Code { get; }

        public SubmissionType SubmissionType { get; }

        public override string ToString() => $"{nameof(SubmitCode)}: {Code.TruncateForDisplay()}";

        internal bool SuppressSplit { get; set; }
    }
}