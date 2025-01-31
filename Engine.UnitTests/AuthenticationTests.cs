﻿using NUnit.Framework;
using OpenTap.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTap.UnitTests
{
    public class AuthenticationTests
    {
        [Test]
        public void ParseTokens()
        {
            string response = @"{
    ""access_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJIaHlId25IdDRnclRDYVhtRHNlSVhHX2U3ajVNb3YzakhLTjZWVlZsZ0lNIn0.eyJleHAiOjE2NDg3MTEwMDQsImlhdCI6MTY0ODcxMDcwNCwianRpIjoiM2NkZDRkYzEtMGE2Mi00YzBjLTljNzQtMmFhZTUwNDk2YWM1IiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiMWY1NmRkMGItNzFkOS00MjAwLWI0YzYtZDE5NWNlNGUxYzJiIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZGVubmlzIiwic2Vzc2lvbl9zdGF0ZSI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImFjciI6IjEiLCJyZWFsbV9hY2Nlc3MiOnsicm9sZXMiOlsiZGVmYXVsdC1yb2xlcy1rczg1MDAiLCJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsInNpZCI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImNsaWVudElkIjoiZGVubmlzIiwiY2xpZW50SG9zdCI6IjEwLjE0OS4xMDkuMjUyIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzZXJ2aWNlLWFjY291bnQtZGVubmlzIiwiY2xpZW50QWRkcmVzcyI6IjEwLjE0OS4xMDkuMjUyIn0.GWKY9EV4sqpLg0GGfmqnGcfjBGhN2NunJrfIysaeRJaYnfsmo3rt-_awpbg15q6IXFipr8N6kE965Y0rxeODxAmRVIf8pb-GkaT0qMOpUidiZrUz3FC0WDXymH3gBayOaKOIa03qVOn5fURmGV4nbQyuJemgQYXW8fcFQpu8xrsM9leYGzVXU4zdxNR-jSfYq1iNN2je9E-EhlglxmvQnirRcoGJsymLxg0s6M_s6cQnQBOihuKsEPE8C3zBeVoCXYJ3kkY0Q6GGK2e8EoRhvrNQ-pyK58yJv5YEnC6Erxe06tfFBZ_XX596YerAqkI4XlHpuEBddqGP0HSYlwtcjA"",
    ""expires_in"": 300,
    ""refresh_expires_in"": 1800,
    ""refresh_token"": ""eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJmMjk4MTY1ZS01MDE5LTQ2YjQtYTYyNS1hMzhiMGY0MzUwZDgifQ.eyJleHAiOjE2NDg3MTI1MDQsImlhdCI6MTY0ODcxMDcwNCwianRpIjoiOTdhOGE3YmEtMTU0OS00NWU5LTgxN2YtY2E1M2YyNzkzYjcwIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOiJodHRwczovL2tleWNsb2FrLmtzODUwMC5hbGIuaXMua2V5c2lnaHQuY29tL2F1dGgvcmVhbG1zL2tzODUwMCIsInN1YiI6IjFmNTZkZDBiLTcxZDktNDIwMC1iNGM2LWQxOTVjZTRlMWMyYiIsInR5cCI6IlJlZnJlc2giLCJhenAiOiJkZW5uaXMiLCJzZXNzaW9uX3N0YXRlIjoiMDVkN2E2ZjItNzBkZC00OGUzLTgwNmItZGM5Y2NiNWU3ZjNlIiwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsInNpZCI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSJ9.VftS1sPVoP_vrYBRuOy7ZT-J9L8SCofxUH1L_5pI6FU"",
    ""token_type"": ""Bearer"",
    ""id_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJIaHlId25IdDRnclRDYVhtRHNlSVhHX2U3ajVNb3YzakhLTjZWVlZsZ0lNIn0.eyJleHAiOjE2NDg3MTEwMDQsImlhdCI6MTY0ODcxMDcwNCwiYXV0aF90aW1lIjowLCJqdGkiOiIwNDQ1MjJmZi1iZWZmLTQxOWYtOTcyNC0yNWJmMDY5ZTdjMjgiLCJpc3MiOiJodHRwczovL2tleWNsb2FrLmtzODUwMC5hbGIuaXMua2V5c2lnaHQuY29tL2F1dGgvcmVhbG1zL2tzODUwMCIsImF1ZCI6ImRlbm5pcyIsInN1YiI6IjFmNTZkZDBiLTcxZDktNDIwMC1iNGM2LWQxOTVjZTRlMWMyYiIsInR5cCI6IklEIiwiYXpwIjoiZGVubmlzIiwic2Vzc2lvbl9zdGF0ZSI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImF0X2hhc2giOiJvMElRSnRyYW52NjA0RUFuWVFETUVnIiwiYWNyIjoiMSIsInNpZCI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImNsaWVudElkIjoiZGVubmlzIiwiY2xpZW50SG9zdCI6IjEwLjE0OS4xMDkuMjUyIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzZXJ2aWNlLWFjY291bnQtZGVubmlzIiwiY2xpZW50QWRkcmVzcyI6IjEwLjE0OS4xMDkuMjUyIn0.JgqiDXT0aArwz0P6nbN8c6HPuVsmUHPuXeCFEsTjf3VdN0PjX2j8thrkGmBw6dr5bSwpX0LTP1vxZfspIe-rpi5UuiKCAmkY92M8T5A7m3yI8gDvlA3RzjXAnrTu3it436444YpYwV9PlQZipy7pIaaWqDOP4AJbnhGARWNHTMozSBCClmQva50nzjEGFFI4Z2ZI9-SPbETdY1xxAqept7JMLnJuPRw_BXFQc8oYTGnqr7kBm8mQnWoFbFi1DZk7VP7e0sSYaI9H3SWZgc2jNpiAa4tpDx9GnTVV4mtQjxB2Xvl_KwZirszewdnDo83M4TnV79PzIr3aJAjOD7crDw"",
    ""not-before-policy"": 0,
    ""session_state"": ""05d7a6f2-70dd-48e3-806b-dc9ccb5e7f3e"",
    ""scope"": ""openid profile email""
}";
            var ti = TokenInfo.FromResponse(response, "packages.opentap.io");
            Assert.IsNotNull(ti.RefreshToken);


            response = @"{
    ""access_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJIaHlId25IdDRnclRDYVhtRHNlSVhHX2U3ajVNb3YzakhLTjZWVlZsZ0lNIn0.eyJleHAiOjE2NDg3MTEwMDQsImlhdCI6MTY0ODcxMDcwNCwianRpIjoiM2NkZDRkYzEtMGE2Mi00YzBjLTljNzQtMmFhZTUwNDk2YWM1IiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiMWY1NmRkMGItNzFkOS00MjAwLWI0YzYtZDE5NWNlNGUxYzJiIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZGVubmlzIiwic2Vzc2lvbl9zdGF0ZSI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImFjciI6IjEiLCJyZWFsbV9hY2Nlc3MiOnsicm9sZXMiOlsiZGVmYXVsdC1yb2xlcy1rczg1MDAiLCJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsInNpZCI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImNsaWVudElkIjoiZGVubmlzIiwiY2xpZW50SG9zdCI6IjEwLjE0OS4xMDkuMjUyIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzZXJ2aWNlLWFjY291bnQtZGVubmlzIiwiY2xpZW50QWRkcmVzcyI6IjEwLjE0OS4xMDkuMjUyIn0.GWKY9EV4sqpLg0GGfmqnGcfjBGhN2NunJrfIysaeRJaYnfsmo3rt-_awpbg15q6IXFipr8N6kE965Y0rxeODxAmRVIf8pb-GkaT0qMOpUidiZrUz3FC0WDXymH3gBayOaKOIa03qVOn5fURmGV4nbQyuJemgQYXW8fcFQpu8xrsM9leYGzVXU4zdxNR-jSfYq1iNN2je9E-EhlglxmvQnirRcoGJsymLxg0s6M_s6cQnQBOihuKsEPE8C3zBeVoCXYJ3kkY0Q6GGK2e8EoRhvrNQ-pyK58yJv5YEnC6Erxe06tfFBZ_XX596YerAqkI4XlHpuEBddqGP0HSYlwtcjA"",
    ""expires_in"": 300,
    ""refresh_expires_in"": 1800,
    ""refresh_token"": ""eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJmMjk4MTY1ZS01MDE5LTQ2YjQtYTYyNS1hMzhiMGY0MzUwZDgifQ.eyJleHAiOjE2NDg3MTI1MDQsImlhdCI6MTY0ODcxMDcwNCwianRpIjoiOTdhOGE3YmEtMTU0OS00NWU5LTgxN2YtY2E1M2YyNzkzYjcwIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOiJodHRwczovL2tleWNsb2FrLmtzODUwMC5hbGIuaXMua2V5c2lnaHQuY29tL2F1dGgvcmVhbG1zL2tzODUwMCIsInN1YiI6IjFmNTZkZDBiLTcxZDktNDIwMC1iNGM2LWQxOTVjZTRlMWMyYiIsInR5cCI6IlJlZnJlc2giLCJhenAiOiJkZW5uaXMiLCJzZXNzaW9uX3N0YXRlIjoiMDVkN2E2ZjItNzBkZC00OGUzLTgwNmItZGM5Y2NiNWU3ZjNlIiwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsInNpZCI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSJ9.VftS1sPVoP_vrYBRuOy7ZT-J9L8SCofxUH1L_5pI6FU"",
    ""token_type"": ""Bearer"",
    ""not-before-policy"": 0,
    ""session_state"": ""05d7a6f2-70dd-48e3-806b-dc9ccb5e7f3e"",
    ""scope"": ""openid profile email""
}";
            ti = TokenInfo.FromResponse(response, "packages.opentap.io");
            Assert.IsNotNull(ti.RefreshToken);

            response = @"{
    ""access_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJIaHlId25IdDRnclRDYVhtRHNlSVhHX2U3ajVNb3YzakhLTjZWVlZsZ0lNIn0.eyJleHAiOjE2NDg3MTEwMDQsImlhdCI6MTY0ODcxMDcwNCwianRpIjoiM2NkZDRkYzEtMGE2Mi00YzBjLTljNzQtMmFhZTUwNDk2YWM1IiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiMWY1NmRkMGItNzFkOS00MjAwLWI0YzYtZDE5NWNlNGUxYzJiIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiZGVubmlzIiwic2Vzc2lvbl9zdGF0ZSI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImFjciI6IjEiLCJyZWFsbV9hY2Nlc3MiOnsicm9sZXMiOlsiZGVmYXVsdC1yb2xlcy1rczg1MDAiLCJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsInNpZCI6IjA1ZDdhNmYyLTcwZGQtNDhlMy04MDZiLWRjOWNjYjVlN2YzZSIsImNsaWVudElkIjoiZGVubmlzIiwiY2xpZW50SG9zdCI6IjEwLjE0OS4xMDkuMjUyIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzZXJ2aWNlLWFjY291bnQtZGVubmlzIiwiY2xpZW50QWRkcmVzcyI6IjEwLjE0OS4xMDkuMjUyIn0.GWKY9EV4sqpLg0GGfmqnGcfjBGhN2NunJrfIysaeRJaYnfsmo3rt-_awpbg15q6IXFipr8N6kE965Y0rxeODxAmRVIf8pb-GkaT0qMOpUidiZrUz3FC0WDXymH3gBayOaKOIa03qVOn5fURmGV4nbQyuJemgQYXW8fcFQpu8xrsM9leYGzVXU4zdxNR-jSfYq1iNN2je9E-EhlglxmvQnirRcoGJsymLxg0s6M_s6cQnQBOihuKsEPE8C3zBeVoCXYJ3kkY0Q6GGK2e8EoRhvrNQ-pyK58yJv5YEnC6Erxe06tfFBZ_XX596YerAqkI4XlHpuEBddqGP0HSYlwtcjA"",
    ""expires_in"": 300,
    ""token_type"": ""Bearer"",
    ""not-before-policy"": 0,
    ""session_state"": ""05d7a6f2-70dd-48e3-806b-dc9ccb5e7f3e"",
    ""scope"": ""openid profile email""
}";
            ti = TokenInfo.FromResponse(response, "packages.opentap.io");
            Assert.IsNull(ti.RefreshToken);
        }

        [Test]
        public void TokenExpiration()
        {

            string response = @"{
    ""access_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJNTGNwamN3ck9uZnNHZWFheXZ1UVppaVZ5RjRhREVweE1XdU1mdGdScU5rIn0.eyJleHAiOjE2NTcwMTg5NzYsImlhdCI6MTY1NzAxODY3NiwianRpIjoiN2RiNDkzM2UtZGIzOS00MTlhLTgxNTctMzBlOTc0YTc2M2JkIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOlsiY2x0LWtzODUwMC1zZWVkIiwiYWNjb3VudCJdLCJzdWIiOiIxZWI4YmFmNC02OWUwLTQyYTQtYTNlMS0yYmM5ZjZhNGUwNzciLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJjbHQta3M4NTAwLXNlc3Npb24tbWFuYWdlciIsInNlc3Npb25fc3RhdGUiOiIzYzQyYzM5NS1iMDEyLTRhMWMtYTc5YS00NGY1MWQzMzhkOGUiLCJhY3IiOiIxIiwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMta3M4NTAwIiwib2ZmbGluZV9hY2Nlc3MiLCJ1bWFfYXV0aG9yaXphdGlvbiJdfSwicmVzb3VyY2VfYWNjZXNzIjp7ImFjY291bnQiOnsicm9sZXMiOlsibWFuYWdlLWFjY291bnQiLCJtYW5hZ2UtYWNjb3VudC1saW5rcyIsInZpZXctcHJvZmlsZSJdfX0sInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwiLCJzaWQiOiIzYzQyYzM5NS1iMDEyLTRhMWMtYTc5YS00NGY1MWQzMzhkOGUiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwicHJlZmVycmVkX3VzZXJuYW1lIjoiZGV2ZWxvcGVyIn0.AH34kCBwfPbqpXHevgyatejxtIvwZCw11wae_P_1qSKP-1EAhSYDqpepINycnORL7PBqEcbq841ohp-Ih-kPVN7K8MLDIPkHZM-FbTh5BxddjlQT0f_O7inglpwDtNNRCGr3gvVClv4eQJ1HPktyeispiFfLqlXvrDgD9_I4TPuRqRSa_fmY2wGDrMtIFSPWn1DGdpV7_-_vOxMs1eJGPH71Ghvlkhgkywccd3Gvl6RPA8L8wq4bDIlJ4v-5LoyBnznZJEalb4lPiEgOhxHLmRjUEcmXUcKhG7uaf7EgN61XqNhmVBVuTLM3gCH44EFSsNER9B3_6Am1SM_z8P1Lhg"",
    ""expires_in"": 299,
    ""refresh_expires_in"": 1800,
    ""refresh_token"": ""eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI5NzgwMjU4Yi05MTVhLTQ3YzctYTA1Ni02YTkyOGY0MTc1MzkifQ.eyJleHAiOjE2NTcwMjA0NzcsImlhdCI6MTY1NzAxODY3NywianRpIjoiYzQ1ZmJlMGEtZmQ0Zi00ZTA3LWEyNTItODYwZTJjNzI5ZjA3IiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5rczg1MDAuYWxiLmlzLmtleXNpZ2h0LmNvbS9hdXRoL3JlYWxtcy9rczg1MDAiLCJhdWQiOiJodHRwczovL2tleWNsb2FrLmtzODUwMC5hbGIuaXMua2V5c2lnaHQuY29tL2F1dGgvcmVhbG1zL2tzODUwMCIsInN1YiI6IjFlYjhiYWY0LTY5ZTAtNDJhNC1hM2UxLTJiYzlmNmE0ZTA3NyIsInR5cCI6IlJlZnJlc2giLCJhenAiOiJjbHQta3M4NTAwLXNlc3Npb24tbWFuYWdlciIsInNlc3Npb25fc3RhdGUiOiIzYzQyYzM5NS1iMDEyLTRhMWMtYTc5YS00NGY1MWQzMzhkOGUiLCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwic2lkIjoiM2M0MmMzOTUtYjAxMi00YTFjLWE3OWEtNDRmNTFkMzM4ZDhlIn0.OlBx9v0iRviNuCjcYM42SzvrKcpKfGgWpDE-Brf5hvc"",
    ""token_type"": ""Bearer"",
    ""id_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJNTGNwamN3ck9uZnNHZWFheXZ1UVppaVZ5RjRhREVweE1XdU1mdGdScU5rIn0.eyJleHAiOjE2NTcwMTg5NzYsImlhdCI6MTY1NzAxODY3NywiYXV0aF90aW1lIjowLCJqdGkiOiI3ZmM2YjBkZC1iOTBkLTQyOGQtODkwMS1iYzliN2JmZGQ3ZGYiLCJpc3MiOiJodHRwczovL2tleWNsb2FrLmtzODUwMC5hbGIuaXMua2V5c2lnaHQuY29tL2F1dGgvcmVhbG1zL2tzODUwMCIsImF1ZCI6WyJjbHQta3M4NTAwLXNlc3Npb24tbWFuYWdlciIsImNsdC1rczg1MDAtc2VlZCJdLCJzdWIiOiIxZWI4YmFmNC02OWUwLTQyYTQtYTNlMS0yYmM5ZjZhNGUwNzciLCJ0eXAiOiJJRCIsImF6cCI6ImNsdC1rczg1MDAtc2Vzc2lvbi1tYW5hZ2VyIiwic2Vzc2lvbl9zdGF0ZSI6IjNjNDJjMzk1LWIwMTItNGExYy1hNzlhLTQ0ZjUxZDMzOGQ4ZSIsImF0X2hhc2giOiJTSG5fWnVteUpaU3h0WU1SZDFxcVRRIiwiYWNyIjoiMSIsInNpZCI6IjNjNDJjMzk1LWIwMTItNGExYy1hNzlhLTQ0ZjUxZDMzOGQ4ZSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJkZXZlbG9wZXIifQ.ZN4zNnZ_dUeIBs51vi-EFD615J0RkVxG3eQ7Sy0Rv5dHFLE2t-xx7zLE_CXytQtbGe_JCzbmH9KbF9pPQixzuJ_Nq5fR0zHU1SwRK3eetpMIq8KjzHnbTL0SMwMuDsBevkMGIXeBry1CGLInbJHkSayq8vE7XhL0tDmd9od-9mczAeXOILX7Z2n06Iu6V8Ja6BmGYCcOsibvV94alahcmg9mIhRcr4Jlg2Bb3La1zCAHhcFU6qvuIvxLZ-gakmcgQ7EqY7zqEgsDIVc1-o1SxE7owt3qU79PYRvjv1cp6u5s3pLBO9xZZoGcKIybsEXxf6bTB6ZVQXkdnWmguHzDXQ"",
    ""not-before-policy"": 1656576311,
    ""session_state"": ""3c42c395-b012-4a1c-a79a-44f51d338d8e"",
    ""scope"": ""openid profile email""
}";

            var ti = TokenInfo.FromResponse(response, "packages.opentap.io");
            DateTime dateTime = new DateTime(2022, 7, 5, 11, 02, 56);
            TimeSpan timeSpan = ti.Expiration - dateTime;
            Console.WriteLine(dateTime.ToString());
            Console.WriteLine(ti.Expiration.ToString());
            Console.WriteLine(timeSpan.ToString());

            Assert.IsTrue(timeSpan.Days == 0);
            Assert.IsTrue(timeSpan.Hours == 0);
            Assert.IsTrue(timeSpan.Minutes == 0);
            Assert.IsTrue(timeSpan.Seconds == 0);
        }

        [Test]
        public void ParseTokenWithUrlChars()
        {
            // This token has a '-' and '_' in the payload which is not in the normal base64 char set. It is in the base64-URL charset though, and should work.
            string accessToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlQUFuOTYwbFpXYU1tZGliTDg4Q29CVlhZSy1VcEhTeWE0T3Z3d04tQzI4In0.eyJleHAiOjE2NTcwNjI5MzUsImlhdCI6MTY1NzA2Mjg3NSwianRpIjoiMGYwNmM3ZWYtOWI3Yi00NzQ1LWJiNTEtY2U5ZTAzOTdmYWMzIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9tYXN0ZXIiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiM2IwNDk0NzYtMzFlOC00NjljLTk3YmEtMWEwZGFmMzAxOGMzIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoicG9zdG1hbiIsInNlc3Npb25fc3RhdGUiOiI1NmU2NGQ5NC00YmFiLTRhYjYtOGZhOC1lODkzNjU2ZTlmZWIiLCJhY3IiOiIxIiwiYWxsb3dlZC1vcmlnaW5zIjpbImh0dHA6Ly9sb2NhbGhvc3QiXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMtbWFzdGVyIiwib2ZmbGluZV9hY2Nlc3MiLCJ1bWFfYXV0aG9yaXphdGlvbiJdfSwicmVzb3VyY2VfYWNjZXNzIjp7ImFjY291bnQiOnsicm9sZXMiOlsibWFuYWdlLWFjY291bnQiLCJtYW5hZ2UtYWNjb3VudC1saW5rcyIsInZpZXctcHJvZmlsZSJdfX0sInNjb3BlIjoicHJvZmlsZSBlbWFpbCIsInNpZCI6IjU2ZTY0ZDk0LTRiYWItNGFiNi04ZmE4LWU4OTM2NTZlOWZlYiIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwibmFtZSI6In5-IDliODM_NTNjIiwicHJlZmVycmVkX3VzZXJuYW1lIjoiYXNnZXIiLCJnaXZlbl9uYW1lIjoifn4iLCJmYW1pbHlfbmFtZSI6IjliODM_NTNjIn0.elXy3abQHHL9-hlVOfkH1JxzgZXyiRSI8JpVJgbiFic7A9fY0qFiUC6aBrR9_FNDU7zh3A4rCAmprdbonMwFRzkRnWfnipXgPTnAtFz9q2i6M0Tcnj-AAgPvZ9sjwtKdOKyzoqoKpEsfdiFYZb31oc8M4R7dRFAixPh8ARv9Lpzx5Hnu7q7A_ewOStQWZbqD-GQvtJyslkbXJM3RaTT3VpDRSXWr67SoIff9SxcrHAJpj_gJcwrg5xrZW4IdmE87_L3LcFvaLzeUx8IvrfpmVHuR8E8yR8RMu2oxiBXD5M1LJCbD3Wx6dTszqFRUOlnR1FA4xAsSgJ8Xba4MB5PWNA";
            var t = new TokenInfo(accessToken, null, "localhost");
            Assert.AreEqual("~~ 9b83?53c", t.Claims["name"]);
            Assert.AreEqual("1", t.Claims["acr"]);
        }
    }
}
