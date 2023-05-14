import axios from 'axios';
import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom';

const initialState = {
    email: '',
    password: ''
}

export default function Login() {

    const [user, setUser] = useState(initialState);
    const [auth, setAuth] = useState(null);
    const navigate = useNavigate();
    const handleOnSubmit = (event) => {
        event.preventDefault();
        alert(user.email);
        axios.post(`https://localhost:44353/api/Auth/Login`, user).then((response) => {
            console.log(response.data)
            const userInfo = response.data;
            if (userInfo.bearerToken !== null) {
                localStorage.setItem('token', response.data.bearerToken);
                sessionStorage.setItem('userInfo', response.data);
                navigate('/', { replace: true });
            }
        }).catch((err) => {
            console.log(err)
        });
    }

    useEffect(() => {
        const userInfo = sessionStorage.getItem('userInfo');
        // if (userInfo === null) {
        //     setAuth(userInfo)
        //     navigate('/login', { replace: true })
        // }else{
        //     navigate('/', { replace: true })
        // }
    }, [])


    return (
        <>
            <div>Login</div>
            <form onSubmit={handleOnSubmit}>
                Email : <input type='email' name='email' value={user.email} onChange={(e) => {
                    setUser({ ...user, email: e.target.value })
                }} />
                <br />
                Password : <input type='password' name='password' value={user.password} onChange={(e) => {
                    setUser({ ...user, password: e.target.value })
                }} />
                <br />
                <input type='submit' name='login' value='Login' />
            </form>
        </>
    )
}
