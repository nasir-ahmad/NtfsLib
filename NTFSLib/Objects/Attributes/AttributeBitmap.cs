﻿using System;
using System.Collections;
using System.Diagnostics;
using NTFSLib.Objects.Enums;

namespace NTFSLib.Objects.Attributes
{
    public class AttributeBitmap : Attribute
    {
        public BitArray Bitfield { get; set; }

        public override AttributeResidentAllow AllowedResidentStates
        {
            get
            {
                return AttributeResidentAllow.Resident | AttributeResidentAllow.NonResident;
            }
        }

        internal override void ParseAttributeResidentBody(byte[] data, int maxLength, int offset)
        {
            base.ParseAttributeResidentBody(data, maxLength, offset);

            Debug.Assert(maxLength >= 1);

            byte[] tmpData = new byte[maxLength];
            Array.Copy(data, offset, tmpData, 0, maxLength);

            Bitfield = new BitArray(tmpData);
        }

        internal override void ParseAttributeNonResidentBody(NTFS ntfs)
        {
            base.ParseAttributeNonResidentBody(ntfs);

            // Get all chunks
            byte[] data = Utils.ReadFragments(ntfs, NonResidentHeader.Fragments);
            
            // Parse
            Bitfield = new BitArray(data);
        }
    }
}