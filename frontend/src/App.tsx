import * as React from "react";
import { Row, Container, Col, Button } from "reactstrap";
import { LoginForm } from "./Login";
import { AddressForm } from "./Address";
import { FullHeightWidth } from "./FullHeightWidth";
import { Show } from "./Show";
import axios from "axios";
import { User } from "./models";
import { ClipLoader } from "react-spinners";
import { getUser, getAllUsers } from "./api";

interface AppState {
  user?: User;
  users?: User[];
  loading: boolean;
}

export class App extends React.PureComponent<{}, AppState> {
  readonly state: AppState = {
    loading: true
  };

  public componentDidMount() {
    this.getData();
  }

  public getData = async () => {
    const { data: users } = await getAllUsers();
    const code = window.localStorage.getItem("code");
    let user = undefined;
    if (code) {
      const { data } = await getUser(code);
      user = data;
    }
    this.setState({ users, user, loading: false });
  };

  public setUser = (user: User | null | undefined) => {
    if (user) {
      console.log("UPDATE", user);
      this.setState({
        user
      });
    }
  };

  public signOut = () => {
    window.localStorage.clear();
    this.setState({ user: undefined });
  };

  public render() {
    const { user, users, loading } = this.state;
    return (
      <FullHeightWidth>
        {loading ? (
          <div style={{ margin: "0 auto" }}>
            <ClipLoader loading={loading} />
          </div>
        ) : (
          <Container>
            {!user && <LoginForm updateUser={this.setUser} />}
            {user && !user.address && (
              <AddressForm user={user} updateUser={this.setUser} />
            )}
            {user && user.address && users && (
              <Show user={user} users={users} />
            )}
            <Row>
              <Col>
                {user && user.address && (
                  <Button onClick={this.signOut}>Sign Out</Button>
                )}
              </Col>
            </Row>
          </Container>
        )}
      </FullHeightWidth>
    );
  }
}
