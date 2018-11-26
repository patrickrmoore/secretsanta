import * as React from "react";
import { Row, Col } from "reactstrap";
import axios from "axios";
import { User } from "./models";
import { FormContainer } from "./FormContainer";
import { keyBy } from "lodash";

interface ShowProps {
  users: User[];
  user: User;
}

export class Show extends React.PureComponent<ShowProps> {
  public render() {
    const { user, users } = this.props;
    const userHash = keyBy(users, "code");
    const givingToUser = userHash[user.givingToId];
    return (
      <Row>
        <Col sm={12}>
          <p>You're the Secret Santa for</p>
          <h4>{givingToUser.name}</h4>
          <p>{givingToUser.address || "Their address will be texted to you"}</p>
        </Col>
      </Row>
    );
  }
}
