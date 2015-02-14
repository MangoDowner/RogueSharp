﻿using System.Collections.Generic;
using System.Linq;
using RogueSharp.DiceNotation.Terms;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   /// <summary>
   /// The DiceExpression class can be used to fluently add DiceTerms to a collection and then Roll them.
   /// </summary>
   public class DiceExpression
   {
      private readonly IList<IDiceExpressionTerm> _terms;

      /// <summary>
      /// Construct a new DiceExpression class with an empty list of terms
      /// </summary>
      public DiceExpression()
         : this( new IDiceExpressionTerm[] { } )
      { }

      private DiceExpression( IEnumerable<IDiceExpressionTerm> diceTerms )
      {
         _terms = diceTerms.ToList();
      }

      /// <summary>
      /// Add a single Die to this DiceExpression with the specified number of sides and scalar
      /// </summary>
      /// <param name="sides">The number of sides on the Die to add to this DiceExpression</param>
      /// <param name="scalar">The value to multiply the result of the Roll of this Die by</param>
      /// <returns>A DiceExpression representing the previous terms in this DiceExpression plus this newly added Die</returns>
      public DiceExpression Die( int sides, int scalar )
      {
         return Dice( 1, sides, scalar );
      }

      /// <summary>
      /// Add a single Die to this DiceExpression with the specified number of sides
      /// </summary>
      /// <param name="sides">The number of sides on the Die to add to this DiceExpression</param>
      /// <returns>A DiceExpression representing the previous terms in this DiceExpression plus this newly added Die</returns>
      public DiceExpression Die( int sides )
      {
         return Dice( 1, sides );
      }

      /// <summary>
      /// Add multiple Dice to this DiceExpression with the specified parameters
      /// </summary>
      /// <param name="multiplicity">The number of Dice</param>
      /// <param name="sides">The number of sides per Die</param>
      /// <param name="scalar">The value to multiply the result of the Roll of these Dice by</param>
      /// <param name="choose">Optional number of dice to choose out of the total rolled. The highest rolled Dice will be choosen.</param>
      /// <returns>A DiceExpression representing the previous terms in this DiceExpression plus these newly added Dice</returns>
      public DiceExpression Dice( int multiplicity, int sides, int scalar = 1, int? choose = null )
      {
         _terms.Add( new DiceTerm( multiplicity, sides, choose ?? multiplicity, scalar ) );
         return this;
      }

      /// <summary>
      /// Add a constant to this DiceExpression with the specified integer value
      /// </summary>
      /// <param name="constant">An integer constant to add to this DiceExpression</param>
      /// <returns>A DiceExpression representing the previous terms in this DiceExpression plus this newly added Constant</returns>
      public DiceExpression Constant( int constant )
      {
         _terms.Add( new ConstantTerm( constant ) );
         return this;
      }

      /// <summary>
      /// Roll all of the Dice that are part of this DiceExpression
      /// </summary>
      /// <param name="random">Optional parameter that defaults to DotNetRandom. If a different IRandom is provided that RNG will be used to perform the Roll instead.</param>
      /// <returns>A DiceResult representing the results of this Roll</returns>
      public DiceResult Roll( IRandom random = null )
      {
         if ( random == null )
         {
            random = Singleton.DefaultRandom;
         }
         IEnumerable<TermResult> termResults = _terms.SelectMany( t => t.GetResults( random ) ).ToList();
         return new DiceResult( termResults, random );
      }

      /// <summary>
      /// Roll all of the Dice that are part of this DiceExpression, but force all of the rolls to be the lowest possible result
      /// </summary>
      /// <returns>A DiceResult representing the results of this Roll. All dice should have rolled their minimum values</returns>
      public DiceResult MinRoll()
      {
         return Roll( new MinRandom() );
      }

      /// <summary>
      /// Roll all of the Dice that are part of this DiceExpression, but force all of the rolls to be the highest possible result
      /// </summary>
      /// <returns>A DiceResult representing the results of this Roll. All dice should have rolled their maximum values</returns>
      public DiceResult MaxRoll()
      {
         return Roll( new MaxRandom() );
      }

      /// <summary>
      /// Returns a string that represents this DiceExpression
      /// </summary>
      /// <returns>A string representing this DiceExpression</returns>
      public override string ToString()
      {
         return string.Join( " + ", _terms );
      }
   }
}