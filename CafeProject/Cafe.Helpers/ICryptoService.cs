﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Helpers
{
    public interface ICryptoService
    {
        /// <summary>
        /// Gets or sets the number of iterations the hash will go through
        /// </summary>
        int HashIterations { get; set; }

        /// <summary>
        /// Gets or sets the size of salt that will be generated if no Salt was set
        /// </summary>
        int SaltSize { get; set; }

        /// <summary>
        /// Gets or sets the plain text to be hashed
        /// </summary>
        string PlainText { get; set; }

        /// <summary>
        /// Gets the base 64 encoded string of the hashed PlainText
        /// </summary>
        string HashedText { get; }

        /// <summary>
        /// Gets or sets the salt that will be used in computing the HashedText
        /// </summary>
        string Salt { get; }

        /// <summary>
        /// Compute the hash
        /// </summary>
        /// <returns>the computed hash: HashedText</returns>
        string Compute();

        /// <summary>
        /// Compute the hash using default generated salt
        /// </summary>
        /// <param name="textToHash"></param>
        /// <returns></returns>
        string Compute(string textToHash);


        /// <summary>
        /// Compute the hash that will utilize the passed salt
        /// </summary>
        /// <param name="textToHash">The text to be hashed</param>
        /// <param name="salt">The salt to be used in the computation</param>
        /// <returns>the computed hash: HashedText</returns>
        string Compute(string textToHash, string salt);

        /// <summary>
        /// Get the time in milliseconds it takes to complete the hash for the iterations
        /// </summary>
        /// <param name="iteration"></param>
        /// <returns></returns>
        int GetElapsedTimeForIteration(int iteration);
    }
}
