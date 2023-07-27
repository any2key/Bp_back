export interface APIResponse {
  isOk: boolean;
  code: number;
  message: string;
}
export interface DataResponse<TData> {
  isOk: boolean;
  code: number;
  message: string;
  data: TData;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
  userId: string;
  login: string;
  userRole: string;
}

export class LoginRequest {

  constructor(l: string, p: string) {
    this.login = l;
    this.password = p;
  }

  login!: string;
  password!: string;
}

export class SignupRequest {
  email!: string;
  login!: string;
  role!: string;
  password!: string;
}

export class RefreshTokenRequest {
  userId!: number;
  refreshToken!: string;
}

export const environment = {

  production: false,

  apiUrl: 'https://localhost:7088'
  //apiUrl: ''
};

export class User {
  id!: number;
  login!: string;
  email!: string;
  role!: string;
  active!: boolean;
  password!: string;
  passwordsalt!: string;
  refreshtokens!: any[];
}

export enum AddOrUpdate {
  add,
  update
}
export class DialogResult<T>
{
  isOk!: boolean;
  data!: T;
}

export class mSettings<T>
{
  value!: T;
  key!: string;
}


export interface Dashboard {
  totalHubs: number;
  onHubs: number;
  servers: number;
  players: number;
}

export interface Server {
  id: string;
  port: number;
}

export interface Hub {
  id: string;
  url: string;
  name: string;
  added: string;
  port: number;
  user: User;
  servers: Server[] | null;
  active: boolean;
}

