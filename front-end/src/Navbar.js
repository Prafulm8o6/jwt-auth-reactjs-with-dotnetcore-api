import React from 'react'
import { NavLink } from 'react-router-dom'

export default function Navbar() {
    return (
        <nav style={{ }}>

            <NavLink to='/'>
                Home 
            </NavLink>

            <NavLink to='/login'>
                Login
            </NavLink>

            <NavLink to='/register'>
                Register
            </NavLink>
        </nav>
    )
}
