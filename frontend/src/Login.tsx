import * as React from "react";
import { Form, Input, Button, FormGroup } from "reactstrap";
import axios from "axios";
import { User } from "./models";
import { FormContainer } from "./FormContainer";

interface LoginFormProps {
  handleLogin: (user: User | null | undefined) => any;
}

interface LoginFormState {
  code: string;
}

export class LoginForm extends React.PureComponent<
  LoginFormProps,
  LoginFormState
> {
  public state = {
    code: ""
  };

  public onLogin = async (code: string) => {
    const response = await axios.get<User>(
      "http://localhost:7071/api/authenticate",
      { params: { code } }
    );
    if (response.status === 200) {
      this.props.handleLogin(response.data);
    }
  };

  public render() {
    return (
      <FormContainer>
        <Form
          onSubmit={(e: React.FormEvent) => {
            e.preventDefault();
            this.onLogin(this.state.code);
          }}
        >
          <FormGroup>
            <Input
              type="text"
              placeholder="Access Code"
              value={this.state.code}
              onChange={e => this.setState({ code: e.target.value })}
            />
          </FormGroup>
          <Button>Login</Button>
        </Form>
      </FormContainer>
    );
  }
}
