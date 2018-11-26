import * as React from "react";
import { Form, Input, Button, FormGroup, Row, Col } from "reactstrap";
import axios from "axios";
import { User } from "./models";
import { FormContainer } from "./FormContainer";
import { updateUser } from "./api";

interface AddressFormProps {
  updateUser: (user: User | null | undefined) => any;
  user: User;
}

const initialState = {
  address: "",
  address2: "",
  city: "",
  state: "",
  zip: ""
};

type AddressFormState = Readonly<typeof initialState>;

export class AddressForm extends React.PureComponent<
  AddressFormProps,
  AddressFormState
> {
  readonly state: AddressFormState = initialState;

  public onAddressSubmit = async () => {
    const { user } = this.props;
    const state = this.state;

    const newUser = {
      ...user,
      address: `${state.address},\n${
        state.address2 ? state.address2 + ",\n" : ""
      }${state.city}, ${state.state} ${state.zip}`
    };
    const response = await updateUser(newUser);
    if (response.status === 200) {
      this.props.updateUser(response.data);
    }
  };

  public render() {
    return (
      <FormContainer>
        <p>
          Hi {this.props.user.name}, set your address to see who your secret
          santa match is.
        </p>
        <Form
          onSubmit={(e: React.FormEvent) => {
            e.preventDefault();
            this.onAddressSubmit();
          }}
        >
          <FormGroup>
            <Input
              type="text"
              placeholder="Address"
              value={this.state.address}
              onChange={e => this.setState({ address: e.target.value })}
            />
          </FormGroup>
          <FormGroup>
            <Input
              type="text"
              placeholder="Address Line 2"
              value={this.state.address2}
              onChange={e => this.setState({ address2: e.target.value })}
            />
          </FormGroup>
          <Row
            // @ts-ignore
            form
          >
            <Col sm={6}>
              <FormGroup>
                <Input
                  type="text"
                  placeholder="City"
                  value={this.state.city}
                  onChange={e => this.setState({ city: e.target.value })}
                />
              </FormGroup>
            </Col>
            <Col sm={6}>
              <FormGroup>
                <Input
                  type="text"
                  placeholder="State"
                  value={this.state.state}
                  onChange={e => this.setState({ state: e.target.value })}
                />
              </FormGroup>
            </Col>
          </Row>
          <FormGroup>
            <Input
              type="number"
              placeholder="Zip"
              value={this.state.zip}
              onChange={e => this.setState({ zip: e.target.value })}
            />
          </FormGroup>
          <Button>Submit</Button>
        </Form>
      </FormContainer>
    );
  }
}
