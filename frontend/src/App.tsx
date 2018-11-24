import * as React from "react";
import { Row, Container, Col } from "reactstrap";
import { LoginForm } from "./Login";
import { AddressForm } from "./Address";
import { FullHeightWidth } from "./FullHeightWidth";
import { Show } from "./Show";
import axios from "axios";
import { User } from "./models";

interface AppState {
  user?: User;
  isLoggedIn: boolean;
  isAddressEntered: boolean;
}

export class App extends React.PureComponent<{}, AppState> {
  public state: AppState = {
    isLoggedIn: false,
    isAddressEntered: false
  };

  public setUser = (user: User | null | undefined) => {
    if (user) {
      this.setState({
        user,
        isLoggedIn: true,
        isAddressEntered: user.address ? true : false
      });
    }
  };

  public render() {
    const { isLoggedIn, isAddressEntered, user } = this.state;
    return (
      <FullHeightWidth>
        <Container>
          {!isLoggedIn && <LoginForm handleLogin={this.setUser} />}
          {isLoggedIn && !isAddressEntered && (
            <AddressForm user={user} handleAddress={this.setUser} />
          )}
          {isLoggedIn && isAddressEntered && user && <Show user={user} />}
        </Container>
      </FullHeightWidth>
    );
  }
}
