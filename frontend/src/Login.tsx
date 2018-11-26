import * as React from "react";
import { Form, Input, Button, FormGroup } from "reactstrap";
import axios from "axios";
import { User } from "./models";
import { FormContainer } from "./FormContainer";
import { authenticate } from "./api";
import LogRocket from "logrocket";

interface LoginFormProps {
  updateUser: (user: User | null | undefined) => any;
}

interface LoginFormState {
  code: string;
  valid: boolean;
}

export class LoginForm extends React.PureComponent<
  LoginFormProps,
  LoginFormState
> {
  public state = {
    code: "",
    valid: true
  };

  public onLogin = async (code: string) => {
    const response = await authenticate(code);
    if (response.status === 200) {
      window.localStorage.setItem("code", code);
      const user = response.data;
      this.props.updateUser(user);
      LogRocket.identify(user.code.toString(), {
        name: user.name
      });
    } else {
      this.setState({ valid: false });
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
              invalid={!this.state.valid}
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
