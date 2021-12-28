using System;
using Godot.NativeInterop;

namespace Godot
{
    /// <summary>
    /// StringNames are immutable strings designed for general-purpose representation of unique names.
    /// StringName ensures that only one instance of a given name exists (so two StringNames with the
    /// same value are the same object).
    /// Comparing them is much faster than with regular strings, because only the pointers are compared,
    /// not the whole strings.
    /// </summary>
    public sealed class StringName : IDisposable
    {
        internal godot_string_name.movable NativeValue;

        ~StringName()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of this <see cref="StringName"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            // Always dispose `NativeValue` even if disposing is true
            NativeValue.DangerousSelfRef.Dispose();
        }

        private StringName(godot_string_name nativeValueToOwn)
        {
            NativeValue = (godot_string_name.movable)nativeValueToOwn;
        }

        // Explicit name to make it very clear
        internal static StringName CreateTakingOwnershipOfDisposableValue(godot_string_name nativeValueToOwn)
            => new StringName(nativeValueToOwn);

        /// <summary>
        /// Constructs an empty <see cref="StringName"/>.
        /// </summary>
        public StringName()
        {
        }

        /// <summary>
        /// Constructs a <see cref="StringName"/> from the given <paramref name="path"/> string.
        /// </summary>
        /// <param name="path">String to construct the <see cref="StringName"/> from.</param>
        public StringName(string name)
        {
            if (!string.IsNullOrEmpty(name))
                NativeValue = (godot_string_name.movable)NativeFuncs.godotsharp_string_name_new_from_string(name);
        }

        /// <summary>
        /// Converts a string to a <see cref="StringName"/>.
        /// </summary>
        /// <param name="from">The string to convert.</param>
        public static implicit operator StringName(string from) => new StringName(from);

        /// <summary>
        /// Converts a <see cref="StringName"/> to a string.
        /// </summary>
        /// <param name="from">The <see cref="StringName"/> to convert.</param>
        public static implicit operator string(StringName from) => from?.ToString();

        /// <summary>
        /// Converts this <see cref="StringName"/> to a string.
        /// </summary>
        /// <returns>A string representation of this <see cref="StringName"/>.</returns>
        public override string ToString()
        {
            if (IsEmpty)
                return string.Empty;

            var src = (godot_string_name)NativeValue;
            NativeFuncs.godotsharp_string_name_as_string(out godot_string dest, src);
            using (dest)
                return Marshaling.ConvertStringToManaged(dest);
        }

        /// <summary>
        /// Check whether this <see cref="StringName"/> is empty.
        /// </summary>
        /// <returns>If the <see cref="StringName"/> is empty.</returns>
        public bool IsEmpty => NativeValue.DangerousSelfRef.IsEmpty;
    }
}
