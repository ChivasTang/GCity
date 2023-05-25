namespace RestAPI.Domains

type RegisterReq =
    { Username: string
      Email: string
      PhoneNumber: string
      Password: string
      Confirm: string }
