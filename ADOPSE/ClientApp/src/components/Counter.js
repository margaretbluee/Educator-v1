import React, { Component } from "react";
import Modules from "./modules";

export class Counter extends Component {
  static displayName = Counter.name;

  constructor(props) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1,
    });
  }

  render() {
    return (
      <Modules />
      // <div>
      //   <h1>Counter</h1>

      //   <p>This is a small weird 2 up example of a React component.</p>

      //   <p aria-live="polite">
      //     Current count: <strong>{this.state.currentCount}</strong>
      //   </p>

      //   <button className="btn btn-primary" onClick={this.incrementCounter}>
      //     Increment
      //   </button>
      // </div>
    );
  }
}
