﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   [TestClass]
   public class SequenceTest
   {
      [TestMethod]
      public void Tick_WhenSequenceHasTwoChildrenReturningSuccess_WillReturnSuccess()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Success;
         mockBehavior2.ReturnStatus = Status.Success;
         Sequence sequence = new Sequence();
         sequence.AddChild( mockBehavior1 );
         sequence.AddChild( mockBehavior2 );

         Status actualStatus = sequence.Tick();

         Assert.AreEqual( Status.Success, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior2.TerminateCalled );
      }

      [TestMethod]
      public void Tick_WhenSequenceHasTwoChildrenWithFirstReturningRunning_WillReturnRunning()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Running;
         mockBehavior2.ReturnStatus = Status.Success;
         Sequence sequence = new Sequence();
         sequence.AddChild( mockBehavior1 );
         sequence.AddChild( mockBehavior2 );

         Status actualStatus = sequence.Tick();

         Assert.AreEqual( Status.Running, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 0, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 0, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 0, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 0, mockBehavior2.TerminateCalled );
      }

      [TestMethod]
      public void Tick_CalledTwiceWhenSequenceHasThreeChildrenWithSecondReturningRunningThenSuccess_WillReturnSuccess()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         MockBehavior mockBehavior3 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Success;
         mockBehavior2.ReturnStatus = Status.Running;
         mockBehavior3.ReturnStatus = Status.Success;
         Sequence sequence = new Sequence();
         sequence.AddChild( mockBehavior1 );
         sequence.AddChild( mockBehavior2 );
         sequence.AddChild( mockBehavior3 );

         sequence.Tick();
         mockBehavior2.ReturnStatus = Status.Success;
         Status actualStatus = sequence.Tick();

         Assert.AreEqual( Status.Success, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 2, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior2.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior3.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior3.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior3.TerminateCalled );
      }

      [TestMethod]
      public void Tick_WhenSequenceHasThreeChildrenWithSecondReturningFailure_WillReturnFailure()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         MockBehavior mockBehavior3 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Success;
         mockBehavior2.ReturnStatus = Status.Failure;
         mockBehavior3.ReturnStatus = Status.Success;
         Sequence sequence = new Sequence();
         sequence.AddChild( mockBehavior1 );
         sequence.AddChild( mockBehavior2 );
         sequence.AddChild( mockBehavior3 );

         Status actualStatus = sequence.Tick();

         Assert.AreEqual( Status.Failure, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior2.TerminateCalled );
         Assert.AreEqual( 0, mockBehavior3.TerminateCalled );
         Assert.AreEqual( 0, mockBehavior3.TerminateCalled );
         Assert.AreEqual( 0, mockBehavior3.TerminateCalled );
      }
   }
}