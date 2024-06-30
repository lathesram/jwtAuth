export interface LoginReqDto {
  email: string;
  password: string;
}

export interface LoginResDto {
  email: string;
  username: string;
  accessToken: string;
  accessTokenExpiration: string;
  refreshToken: string;
}

export interface RegisterReqDto {
  email: string;
  username: string;
  password: string;
}

export interface RefreshTokenReqDto {
  refreshTokenString: string
}

export interface LoggedInUser extends LoginResDto {}
