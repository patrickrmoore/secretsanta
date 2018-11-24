import * as React from "react";
import { Row, Col } from "reactstrap";
import axios from "axios";
import { User } from "./models";
import { FormContainer } from "./FormContainer";

interface ShowProps {
  user: User;
}

export class Show extends React.PureComponent<ShowProps> {
  public render() {
    return (
      <Row>
        <Col sm={12}>
          <p>You're the Secret Santa for</p>
          <h4>{this.props.user.givingToId}</h4>
          <p>Their address will be texted to you.</p>
        </Col>
      </Row>
    );
  }
}
